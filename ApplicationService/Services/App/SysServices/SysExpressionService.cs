
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using System.Linq.Expressions;
using System.Reflection;

namespace ApplicationService;

public class SysExpressionService : BaseService<SysExpression, Guid, ApplicationDbContext>, ISysExpressionService
{
    public SysExpressionService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<DtoSysExpression>> GetAllAsync()
    {
        return await _repository.GetListAsync(s => new DtoSysExpression
        {
            Id = s.Id,
            Language = s.Language,
            ExpressionType = s.ExpressionType,
            ExpressionItem = s.ExpressionItem,
            Equivalent = s.Equivalent
        });
    }

    public async Task<DtoSysExpression?> GetAsync(Guid id)
    {
        return await _repository.FirstAsync(c => c.Id == id, s => new DtoSysExpression
        {
            Id = s.Id,
            Language = s.Language,
            ExpressionType = s.ExpressionType,
            ExpressionItem = s.ExpressionItem,
            Equivalent = s.Equivalent
        });
    }

    public void FetchAllExpressions()
    {
        SysExpressionHelper.Items = _repository.GetList(s => new DtoSysExpression
        {
            Id = s.Id,
            Language = s.Language,
            ExpressionType = s.ExpressionType,
            ExpressionItem = s.ExpressionItem,
            Equivalent = s.Equivalent
        }).ToList();
    }

    public async Task<ServiceResult> UpdateEquivalentAsync(Guid id, string equivalent, SysCustomUser user)
    {
        var model = new SysExpression
        {
            Id = id,
            Equivalent = equivalent,
        };

        _repository.Update(model, new List<Expression<Func<SysExpression, object>>>() { c => c.Equivalent }, user);
        return await SaveAsync(model, user, CrudType.Update);
    }

    public async Task CheckMappingsValidity(Language language, bool insertMissedOnes)
    {
        var mappings = GetAllPattern();

        var existMappings = await _repository.GetListAsync(c => c.Language == language);
        var missedMappings = mappings.Where(c => !existMappings.Any(m => m.ExpressionType == c.Item1 && m.ExpressionItem == c.Item2)).ToList();
        if (insertMissedOnes && missedMappings.Any())
            await InsertMissedMappings(missedMappings, language);
    }

    private List<Tuple<ExpressionTypes, int>> GetAllPattern()
    {
        var result = new List<Tuple<ExpressionTypes, int>>();

        foreach (ExpressionTypes item in Enum.GetValues(typeof(ExpressionTypes)))
        {
            var relatedEnum = typeof(ExpressionTypes).GetField(item.ToString())?.GetCustomAttribute<RelatedEnum>();
            if (relatedEnum is null)
                continue;

            foreach (var relatedEnumItem in Enum.GetValues(relatedEnum.EnumType))
                result.Add(new Tuple<ExpressionTypes, int>(item, (int)relatedEnumItem));
        }
        return result;
    }

    private async Task InsertMissedMappings(List<Tuple<ExpressionTypes, int>> missedMappings, Language language)
    {
        foreach (var missedMapping in missedMappings)
        {
            var sysExpression = new SysExpression
            {
                Language = language,
                ExpressionType = missedMapping.Item1,
                ExpressionItem = missedMapping.Item2,
            };

            if (language == Language.English)
                sysExpression.Equivalent = SysExpressionHelper.FindPureName(missedMapping.Item1, missedMapping.Item2);

            await CreateAsync(sysExpression, GeneralVariables.SystemUser);
        }
    }
}