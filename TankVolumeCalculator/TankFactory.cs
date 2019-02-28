using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankVolumeCalculator
{
    class TankFactory
    {
        public Tank GetTank(TankType type)
        {
            Tank tank = null;
            switch (type)
            {
                case TankType.RegularTank:
                    tank = new RegularTank();
                    break;
                case TankType.NonRegularTank:
                    tank = new NonRegularTank();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return tank;
        }
    }
}
