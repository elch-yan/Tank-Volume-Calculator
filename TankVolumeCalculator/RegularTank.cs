using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankVolumeCalculator
{
    class RegularTank : Tank
    {
        [Label("Cylinder Length")]
        public Double CylindricPartLength { set; get; }

        [Label("Head Depth")]
        public Double HeadDepth { set; get; }

        public override double CalculateVolume(double liquidHeight)
        {
            return CalculateHeadsVolume(liquidHeight) + CalculateCylinderVolume(liquidHeight);
        }

        private double CalculateCylinderVolume(double liquidHeight)
        {
            double liquidHeightOnDiameter = liquidHeight / Height;
            double area = square(Height) * (0.25 * Math.Acos(1 - 2 * liquidHeightOnDiameter) -
                (0.5 - liquidHeightOnDiameter) * Math.Sqrt(liquidHeightOnDiameter * (1 - liquidHeightOnDiameter)));

            return area * CylindricPartLength;
        }

        private double CalculateHeadsVolume(double liquidHeight)
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
                double currentLayerArea = CalculateHeadsLayerArea(i + 1);
                volume += CalculateLayerVolume(previousLayerArea, currentLayerArea);
                previousLayerArea = currentLayerArea;
            }

            if (liquidMoreThanHalf)
            {
                for (; i < tankHalfLayersCount; i++)
                {
                    double currentLayerArea = CalculateHeadsLayerArea(i + 1);
                    volume += 2 * CalculateLayerVolume(previousLayerArea, currentLayerArea);
                    previousLayerArea = currentLayerArea;
                }
            }

            return volume;
        }

        private double CalculateHeadsLayerArea(int layerIndex)
        {
            double vesselRadius = Height / 2;
            double layerHeight = VOLUME_CALCULATION_STEP * layerIndex;

            double rectangleHalfLength = Math.Sqrt(square(vesselRadius) - square(vesselRadius - layerHeight));
            double layerHeadDepth = HeadDepth * Math.Sqrt(1 - square((vesselRadius - layerHeight) / vesselRadius));

            return rectangleHalfLength * Math.PI * layerHeadDepth;
        }

        public override string GetInfo()
        {
            return "Height: " + Height + "\tCylinder Length: " + CylindricPartLength + "\tHead depth: " + HeadDepth;
        }
    }
}
