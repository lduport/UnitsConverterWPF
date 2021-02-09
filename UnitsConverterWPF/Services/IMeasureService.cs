using System;
using System.Collections.Generic;
using UnitsNet;

namespace UnitsConverterWPF.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMeasureService
    {
        IEnumerable<QuantityInfo> GetMeasures(string filter);

        IEnumerable<UnitInfo> GetUnits(QuantityInfo quantityInfo);

        double Convert(QuantityValue fromValue, Enum fromUnitValue, Enum toUnitValue);
    }
}
