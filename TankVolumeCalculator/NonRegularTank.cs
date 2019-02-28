using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankVolumeCalculator
{
    class NonRegularTank : Tank
    {
        [Label("Length")]
        public Double Length { set; get; }

        [Label("Width")]
        public Double Width { set; get; }

        public override double CalculateVolume(double liquidHeight)
        {
            int layersCount = (int)(liquidHeight / VOLUME_CALCULATION_STEP);
            int tankHalfLayersCount = (int)(Height / (VOLUME_CALCULATION_STEP * 2));

            bool liquidMoreThanHalf = false;

            if (layersCount > tankHalfLayersCount)
            {
                liquidMoreThanHalf = true;
                layersCount = 2 * tankHalfLayersCount - layersCount;
            }

            double volume = 0;
            double previousLayerArea = 0;
            int i;
            for (i = 0; i < layersCount; i++)
            {
                double currentLayerArea = CalculateLayerArea(i + 1);
                volume += CalculateLayerVolume(previousLayerArea, currentLayerArea);
                previousLayerArea = currentLayerArea;
            }

            if (liquidMoreThanHalf)
            {
                for (; i < tankHalfLayersCount; i++)
                {
                    double currentLayerArea = CalculateLayerArea(i + 1);
                    volume += 2 * CalculateLayerVolume(previousLayerArea, currentLayerArea);
                    previousLayerArea = currentLayerArea;
                }
            }

            return volume;
        }

        private double CalculateLayerArea(int layerIndex)
        {
            double vesselRadius = Height / 2;
            double layerHeight = VOLUME_CALCULATION_STEP * layerIndex;

            double layerWidth = Width * Math.Sqrt(1 - square((vesselRadius - layerHeight) / vesselRadius));

            return Length * layerWidth;
        }

        public override string GetInfo()
        {
            return "Height: " + Height + "\tLength: " + Length + "\tCube width: " + Width;
        }
    }
}