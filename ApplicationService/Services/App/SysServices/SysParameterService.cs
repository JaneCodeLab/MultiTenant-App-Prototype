
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using System.Linq.Expressions;
using System.Reflection;

namespace ApplicationService;

public class SysParameterService : BaseService<SysParameter, Guid, ApplicationDbContext>, ISysParameterService
{
    public SysParameterService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<DtoSysParameter>> GetAllAsync()
    {
        return await _repository.GetListAsync(s => new DtoSysParameter
        {
            Id = s.Id,
            Language = s.Language,
            ParameterType = s.ParameterType,
            ParameterItem = s.ParameterItem,
            Equivalent = s.Equivalent
        });
    }

    public async Task<DtoSysParameter?> GetAsync(Guid id)
    {
        return await _repository.FirstAsync(c => c.Id == id, s => new DtoSysParameter
        {
            Id = s.Id,
            Language = s.Language,
            ParameterType = s.ParameterType,
            ParameterItem = s.ParameterItem,
            Equivalent = s.Equivalent
        });
    }

    public void FetchAllParameters()
    {
        SysParameterHelper.Items = _repository.GetList(s => new SysParameterModel
        {
            Language = s.Language,
            ParameterType = s.ParameterType,
            ParameterItem = s.ParameterItem,
            Equivalent = s.Equivalent
        }).ToList();
    }

    public async Task<ServiceResult> UpdateEquivalentAsync(Guid id, string equivalent, SysCustomUser user)
    {
        var model = new SysParameter
        {
            Id = id,
            Equivalent = equivalent,
        };

        _repository.Update(model, new List<Expression<Func<SysParameter, object>>>() { c => c.Equivalent }, user);
        return await SaveAsync(model, user, CrudType.Update);
    }

    public async Task CheckMappingsValidity(Language language, bool insertMissedOnes)
    {
        var mappings = GetAllPattern();

        var existMappings = await _repository.GetListAsync(c => c.Language == language);
        var missedMappings = mappings.Where(c => !existMappings.Any(m => m.ParameterType == c.Item1 && m.ParameterItem == c.Item2)).ToList();
        if (insertMissedOnes && missedMappings.Any())
            await InsertMissedMappings(missedMappings, language);
    }

    private List<Tuple<ParameterTypes, int>> GetAllPattern()
    {
        var result = new List<Tuple<ParameterTypes, int>>();

        foreach (ParameterTypes item in Enum.GetValues(typeof(ParameterTypes)))
        {
            var relatedEnum = typeof(ParameterTypes).GetField(item.ToString())?.GetCustomAttribute<RelatedEnum>();
            if (relatedEnum is null)
                continue;

            foreach (var relatedEnumItem in Enum.GetValues(relatedEnum.EnumType))
                result.Add(new Tuple<ParameterTypes, int>(item, (int)relatedEnumItem));
        }
        return result;
    }

    private async Task InsertMissedMappings(List<Tuple<ParameterTypes, int>> missedMappings, Language language)
    {
        foreach (var missedMapping in missedMappings)
        {
            var newParam = new SysParameter
            {
                Language = language,
                ParameterType = missedMapping.Item1,
                ParameterItem = missedMapping.Item2,
            };

            if (language == Language.English)
                newParam.Equivalent = SysParameterHelper.FindPureName(missedMapping.Item1, missedMapping.Item2);

            await CreateAsync(newParam, GeneralVariables.SystemUser);
        }
    }
}