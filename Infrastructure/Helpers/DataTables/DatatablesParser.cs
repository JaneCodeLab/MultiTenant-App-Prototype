
using ApplicationCore;
using Microsoft.Extensions.Primitives;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers.DataTables;

public class DatatablesParser<T, DTO>
        where T : class
        where DTO : class, new()
{
    private readonly Dictionary<string, string> _config;
    private readonly Type _tType;
    private readonly Type _dtoType;
    private IDictionary<int, PropertyMap> _propertyMap;

    //Global configs
    private int _take;
    private int _skip;
    private bool _sortDisabled = false;

    public DatatablesParser(IEnumerable<KeyValuePair<string, StringValues>> configParams)
    {
        _config = configParams.ToDictionary(k => k.Key, v => v.Value.First().Trim());
        _tType = typeof(T);
        _dtoType = typeof(DTO);
        var combinedProps = _tType.GetProperties().ToList()
                   .Concat(_dtoType.GetProperties().ToList());

        _propertyMap = _config
            .Join(combinedProps,
                    param => param.Value,
                    prop => prop.Name,
                    (param, prop) => new
                    {
                        param,
                        prop
                    })
            .Where(a => Regex.IsMatch(a.param.Key, DataTableConstants.COLUMN_PROPERTY_PATTERN))

            .Select(a =>
            {
                var index = Regex.Match(a.param.Key, DataTableConstants.COLUMN_PROPERTY_PATTERN).Groups[1].Value;
                var searchableKey = DataTableConstants.GetKey(DataTableConstants.SEARCHABLE_PROPERTY_FORMAT, index);
                var searchable = _config.ContainsKey(searchableKey) && _config[searchableKey] == "true";
                var orderableKey = DataTableConstants.GetKey(DataTableConstants.ORDERABLE_PROPERTY_FORMAT, index);
                var orderable = _config.ContainsKey(orderableKey) && _config[orderableKey] == "true";
                var filterKey = DataTableConstants.GetKey(DataTableConstants.SEARCH_VALUE_PROPERTY_FORMAT, index);
                var filter = _config.ContainsKey(filterKey) ? _config[filterKey] : string.Empty;
                return new
                {
                    index = int.Parse(index),
                    map = new PropertyMap
                    {
                        Property = a.prop,
                        Searchable = searchable,
                        Orderable = orderable,
                        Filter = filter
                    }
                };
            })
            .GroupBy(p => p.index)
            .Select(g => g.First())
            .Distinct().ToDictionary(k => k.index, v => v.map);


        if (_propertyMap.Count == 0)
        {
            throw new Exception("No properties were found in request. Please map datatable field names to properties in T");
        }

        if (_config.ContainsKey(DataTableConstants.DISPLAY_START))
        {
            int.TryParse(_config[DataTableConstants.DISPLAY_START], out _skip);
        }


        if (_config.ContainsKey(DataTableConstants.DISPLAY_LENGTH))
        {
            int.TryParse(_config[DataTableConstants.DISPLAY_LENGTH], out _take);
        }
        else
        {
            _take = 10;
        }

        _sortDisabled = _config.ContainsKey(DataTableConstants.ORDERING_ENABLED) && _config[DataTableConstants.ORDERING_ENABLED] == "false";
    }

    public DataTablePagination<T> Parse()
    {
        // parse the echo property
        var draw = int.Parse(_config[DataTableConstants.DRAW]);

        var columns = _propertyMap.OrderBy(a => a.Key).Select(kvp => kvp.Value)
            .Select(a => a.Property.Name).ToList();

        OrderBy<T> order = default;
        //sort results if sorting isn't disabled or skip needs to be called
        if (!_sortDisabled || _skip > 0)
        {
            order = ApplySort();
        }


        var hasFilterText = !string.IsNullOrWhiteSpace(_config[DataTableConstants.SEARCH_KEY]) || _propertyMap.Any(p => !string.IsNullOrWhiteSpace(p.Value.Filter));

        int page = _skip / _take + 1;
        return new DataTablePagination<T>()
        {
            OrderBy = order,
            Draw = draw,
            Search = null,
            Size = _take,
            Page = page,
            Columns = columns
        };
    }



    private OrderBy<T> ApplySort()
    {
        var sorted = false;
        var paramExpr = Expression.Parameter(_tType, "Param_0");
        var order = new OrderBy<T>();
        var typeProperties = _tType.GetProperties();
        // Enumerate the keys sort keys in the order we received them
        foreach (var param in _config.Where(k => Regex.IsMatch(k.Key, DataTableConstants.ORDER_PATTERN)))
        {
            // column number to sort (same as the array)
            int sortcolumn = int.Parse(param.Value);

            // ignore disabled columns 
            if (!_propertyMap.ContainsKey(sortcolumn) || !_propertyMap[sortcolumn].Orderable)
            {
                continue;
            }

            var index = Regex.Match(param.Key, DataTableConstants.ORDER_PATTERN).Groups[1].Value;
            var orderDirectionKey = DataTableConstants.GetKey(DataTableConstants.ORDER_DIRECTION_FORMAT, index);

            // get the direction of the sort
            string sortdir = _config[orderDirectionKey];



            // apply the sort (default is ascending if not specified)
            OrderType orderType;
            if (string.IsNullOrEmpty(sortdir) || sortdir.Equals(DataTableConstants.ASCENDING_SORT, StringComparison.OrdinalIgnoreCase))
                orderType = OrderType.Asc;
            else
                orderType = OrderType.Desc;

            var sortProperty = _propertyMap[sortcolumn].Property;

            var mainColumnAttribute = _dtoType.GetProperty(sortProperty.Name)!.GetCustomAttribute<MainColumnAttribute>();
            if (mainColumnAttribute is not null && _tType.GetProperty(mainColumnAttribute?.OrderBy!) != null)
                sortProperty = _tType.GetProperty(mainColumnAttribute?.OrderBy!);


            if (_tType.GetProperty(sortProperty.Name) != null)
            {
                var expression = Expression.Property(paramExpr, sortProperty);
                var conversion = Expression.Convert(expression, typeof(object));
                order = new OrderBy<T> { Orders = new() { Expression.Lambda<Func<T, object>>(conversion, paramExpr) }, OrderType = orderType };
                sorted = true;
            }
        }

        //Linq to entities needs a sort to implement skip
        //Not sure if we care about the queriables that come in sorted? IOrderedQueryable does not seem to be a reliable test
        if (!sorted)
        {
            var firstProp = Expression.Property(paramExpr, typeProperties.First());
            var conversion = Expression.Convert(firstProp, typeof(object));
            order = new OrderBy<T> { Orders = new() { Expression.Lambda<Func<T, object>>(conversion, paramExpr) }, OrderType = OrderType.Desc };
        }
        return order;
    }
}
