using UnitsNet;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UnitsConverterWPF.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class MeasureService : IMeasureService
    {
        public IEnumerable<QuantityInfo> GetMeasures(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Quantity.Infos;
            }

            return Quantity.Infos.Where(q => q.Name.ToLowerInvariant().StartsWith(filter.ToLowerInvariant()));
        }

        public IEnumerable<UnitInfo> GetUnits(QuantityInfo quantityInfo)
        {
            if(quantityInfo == null)
            {
                return Enumerable.Empty<UnitInfo>();
            }

            return quantityInfo.UnitInfos;
        }
    }
}
