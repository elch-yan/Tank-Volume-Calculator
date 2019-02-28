using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankVolumeCalculator
{
    public abstract class Tank
    {
        protected const double VOLUME_CALCULATION_STEP = 0.01;

        [Label("Height")]
        public Double Height { set; get; }

        public abstract Double CalculateVolume(Double liquidHeight);

        protected double CalculateLayerVolume(double lowerLayerArea, double upperLayerArea)
        {
            return (lowerLayerArea + upperLayerArea) * VOLUME_CALCULATION_STEP / 2;
        }

        public abstract String GetInfo();

        protected Double square(double number)
        {
            return number * number;
        }
    }
}
