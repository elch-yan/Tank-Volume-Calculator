using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TankVolumeCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //enum TankType { Hemispherical, ASME_21Elliptical, DIN_28013Semi_Ellipsoidal, ASMETorispherical, DIN_28011Torispherical, Bumped };

        private double liquidHeight;

        private const double FILE_WRITE_STEP = 0.1;

        private const int MAXIMUM_NUMBER_OF_LINES_IN_FILE = 50;

        private Tank tank;

        private TankFactory factory;

        private Dictionary<PropertyInfo, TextBox> propertyToElement;

        //private double vesselWallThickness;

        //private double insideKnuckleRadius;

        //private TankType tankType;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            factory = new TankFactory();
            propertyToElement = new Dictionary<PropertyInfo, TextBox>();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            SetTankFields();

            //double volume = tank.CalculateVolume(liquidHeight);
            //txtCalculation.Text = volume.ToString();
            double volume;

            List<String> lines = new List<String>();
            for (int i = 0; i < Math.Ceiling(tank.Height / FILE_WRITE_STEP); i++)
            {
                liquidHeight = (i + 1) * FILE_WRITE_STEP;
                volume = tank.CalculateVolume(liquidHeight);
                lines.Add(liquidHeight * 10 + "cm\t" + Math.Round(volume) + "L");
            }

            using (StreamWriter file = new StreamWriter(@"Volumes.txt"))
            {
                file.WriteLine(tank.GetInfo());
                file.WriteLine();
                for (int i = 0; i < MAXIMUM_NUMBER_OF_LINES_IN_FILE; i++)
                {
                    int j = i;
                    String line = lines[j];
                    j += MAXIMUM_NUMBER_OF_LINES_IN_FILE;
                    while (j < lines.Count)
                    {
                        line += "\t\t" + lines[j];
                        j += MAXIMUM_NUMBER_OF_LINES_IN_FILE;
                    }

                    file.WriteLine(line);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedValue = (cmbTankType.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedValue == "Regular")
            {
                this.tank = factory.GetTank(TankType.RegularTank);
            }
            else
            {
                if (selectedValue == "Non Regular")
                {
                    this.tank = factory.GetTank(TankType.NonRegularTank);
                }
            }
            BuildTankFields();
        }

        private void BuildTankFields()
        {
            tankGrid.Children.Clear();
            tankGrid.RowDefinitions.Clear();
            propertyToElement.Clear();

            int rowIndex = 0;
            foreach (PropertyInfo pInfo in this.tank.GetType().GetProperties())
            {
                BuildTankField(pInfo, ref rowIndex);
            }
            //window.Height = rowIndex * 74 + 60;
        }

        private void BuildTankField(PropertyInfo pInfo, ref int rowIndex)
        {
            Label lblPropertyName = new Label();
            LabelAttribute labelAttr = pInfo.GetCustomAttributes(typeof(LabelAttribute), true).FirstOrDefault() as LabelAttribute;
            lblPropertyName.Content = labelAttr.Label;
            tankGrid.RowDefinitions.Add(new RowDefinition());
            tankGrid.Children.Add(lblPropertyName);
            Grid.SetRow(lblPropertyName, rowIndex++);
            
            TextBox txtValueBox = new TextBox();
            tankGrid.RowDefinitions.Add(new RowDefinition());
            tankGrid.Children.Add(txtValueBox);
            Grid.SetRow(txtValueBox, rowIndex++);

            propertyToElement.Add(pInfo, txtValueBox);
        }

        private void SetTankFields()
        {
            foreach (PropertyInfo pInfo in propertyToElement.Keys)
            {
                pInfo.SetValue(tank, Double.Parse(propertyToElement[pInfo].Text), null);
            }
        }

        #region comments
        //private void InitializeFirstPhaseElements()
        //{
        //    this.dynamicGrid.RowDefinitions.Clear();
        //    this.dynamicGrid.ColumnDefinitions.Clear();

        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbHemispherical = new CheckBox();
        //    cbHemispherical.Checked += cbHemispherical_Checked;
        //    dynamicGrid.Children.Add(cbHemispherical);
        //    Grid.SetRow(cbHemispherical, 0);
        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbASMEElliptical = new CheckBox();
        //    cbASMEElliptical.Checked += cbASMEElliptical_Checked;
        //    dynamicGrid.Children.Add(cbASMEElliptical);
        //    Grid.SetRow(cbASMEElliptical, 1);
        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbDinEllipsoidal = new CheckBox();
        //    cbDinEllipsoidal.Checked += cbDinEllipsoidal_Checked;
        //    dynamicGrid.Children.Add(cbHemispherical);
        //    Grid.SetRow(cbHemispherical, 0);
        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbHemispherical = new CheckBox();
        //    cbHemispherical.Checked += cbHemispherical_Checked;
        //    dynamicGrid.Children.Add(cbHemispherical);
        //    Grid.SetRow(cbHemispherical, 0);
        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbHemispherical = new CheckBox();
        //    cbHemispherical.Checked += cbHemispherical_Checked;
        //    dynamicGrid.Children.Add(cbHemispherical);
        //    Grid.SetRow(cbHemispherical, 0);
        //    this.dynamicGrid.RowDefinitions.Add(new RowDefinition());
        //    CheckBox cbHemispherical = new CheckBox();
        //    cbHemispherical.Checked += cbHemispherical_Checked;
        //    dynamicGrid.Children.Add(cbHemispherical);
        //    Grid.SetRow(cbHemispherical, 0);
        //}

        //void cbDinEllipsoidal_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.tankType = TankType.DIN_28013Semi_Ellipsoidal;
        //}

        //void cbASMEElliptical_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.tankType = TankType.ASME_21Elliptical;
        //}

        //private void cbHemispherical_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.tankType = TankType.Hemispherical;
        //}

        //private void btnCalculate_Click(object sender, RoutedEventArgs e)
        //{
        //    this.valueFromTextBoxToField(this.txtFluidLevel, ref liquidHeight);
        //    this.valueFromTextBoxToField(this.txtElipseWidth, ref headDepth);
        //    this.valueFromTextBoxToField(this.txtLength, ref tankLength);
        //    this.valueFromTextBoxToField(this.txtHeight, ref vesselDimaeter);
        //}

        //private void valueFromTextBoxToField(TextBox element, ref double field)
        //{
        //    if(!Double.TryParse(element.Text,out field)) 
        //    {
        //        MessageBox.Show("Invalid Parameter in field!");
        //    }
        //}

        //private Double getHemisphericalHeadVolume()
        //{
        //    double liquidHeightOnDiameter = liquidHeight / vesselDimaeter;

        //    return Math.Pow(vesselDimaeter, 3) * Math.PI / 6 * square(liquidHeightOnDiameter) * (3  - 2 * liquidHeightOnDiameter);
        //}

        //private Double getASME_21EllipticalHeadVolume() 
        //{
        //    return 0.5 * getHemisphericalHeadVolume();
        //}

        //private Double getDIN_28013Semi_EllipsoidalHeadVolume()
        //{
        //    double thicknessOnFullDiameter = vesselWallThickness / (2 * vesselWallThickness + vesselDimaeter);
        //    return (0.49951 + thicknessOnFullDiameter * ( 0.10462 + 2.3227 * thicknessOnFullDiameter)) * getHemisphericalHeadVolume();
        //}

        //private Double getASMETorisphericalHeadVolume() 
        //{
        //    double fullDiameter = (2 * vesselWallThickness + vesselDimaeter);
        //    double thicknessOnFullDiameter = vesselWallThickness / fullDiameter;

        //    return (0.30939 + 1.7197 * (insideKnuckleRadius - 0.06 * fullDiameter) / vesselDimaeter + 
        //        thicknessOnFullDiameter * (- 0.16116 + 0.98997 * thicknessOnFullDiameter)) * 
        //        getHemisphericalHeadVolume();
        //}

        //private Double getDIN_28011TorisphericalHeadVolume()
        //{
        //    double thicknessOnFullDiameter = vesselWallThickness / (2 * vesselWallThickness + vesselDimaeter);
        //    return (0.37802 + thicknessOnFullDiameter * (0.05073 + 1.3762 * thicknessOnFullDiameter)) * getHemisphericalHeadVolume();
        //}

        //private Double getBumpedHeadVolume()
        //{
        //    double totalVolume = 0.5 * Math.PI * square(vesselWallThickness) * (3 * vesselDimaeter - vesselWallThickness);

        //    return 3 * totalVolume * square(liquidHeight / vesselDimaeter) * (1 - liquidHeight / (3 * vesselDimaeter));
        //}

        //private Double getCylindricalSectionVolume() 
        //{
        //    double liquidHeightOnDiameter = liquidHeight / vesselDimaeter;
        //    double area = square(vesselDimaeter) * (0.25 * Math.Acos(1 - 2 * liquidHeightOnDiameter) - 
        //        (0.5 - liquidHeightOnDiameter) * Math.Sqrt(liquidHeightOnDiameter * (1 - liquidHeightOnDiameter)));

        //    return area * this.tankLength;
        //}

        #endregion
    }
}