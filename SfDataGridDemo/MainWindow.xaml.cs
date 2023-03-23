using Microsoft.UI.Xaml;
using Syncfusion.UI.Xaml.DataGrid;
using System;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DataGridDemo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //Event subscription
            dataGrid.CurrentCellEndEdit += OnCurrentCellEndEdit;
        }

        //Event customization
        void OnCurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs args)
        {
            //Get DataGrid
            var dataGrid = sender as SfDataGrid;

            if (dataGrid != null)
            {
                //Get the collection of SelectedCells using GetSelectedCells helper method
                var selectedcells = dataGrid.GetSelectedCells();

                if (selectedcells != null && dataGrid.View != null)
                {
                    var propertyAccessProvider = dataGrid.View.GetPropertyAccessProvider();
                    
                    var itemProperties = dataGrid.View.GetItemProperties();

                    if (propertyAccessProvider != null && itemProperties != null)
                    {
                        if (dataGrid.CurrentItem != null && dataGrid.CurrentColumn != null && dataGrid.CurrentColumn.MappingName != null)
                        {
                            //Get the edited value in CurrentItem property
                            var newValue = propertyAccessProvider.GetValue(dataGrid.CurrentItem, dataGrid.CurrentColumn.MappingName);

                            //Check the selectedcells have 0r not
                            if (selectedcells.Count > 0)
                            {
                                try
                                {
                                    selectedcells.ForEach(item =>
                                    {
                                        var cellInfo = item as GridCellInfo;
                                        var propInfo = itemProperties.Find(cellInfo.Column.MappingName, true);
                                        if (propInfo != null && propInfo.PropertyType == newValue.GetType())
                                            propertyAccessProvider.SetValue(cellInfo.RowData, cellInfo.Column.MappingName, newValue);
                                        else if (propInfo != null)
                                        {
                                            var value = Convert.ChangeType(newValue, propInfo.PropertyType);
                                            propertyAccessProvider.SetValue(cellInfo.RowData, cellInfo.Column.MappingName, value);
                                        }
                                    });
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}