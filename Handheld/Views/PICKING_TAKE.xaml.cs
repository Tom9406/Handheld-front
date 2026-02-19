using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using static Handheld.Views.Picking_Line_Details;

namespace Handheld.Views
{
    public partial class PICKING_TAKE : ContentPage, IQueryAttributable
    {
        private List<PickLines> _allDocuments;

        public PICKING_TAKE()
        {
            InitializeComponent();

            // Datos de ejemplo (luego vendrán de BC / API)
            _allDocuments = new List<PickLines>
            {
                new PickLines
                {
                    SourceDocument = "PI000004",
                    Bin = "W-01-001",
                    Item_No = "ITM-TEST-1",
                    Open_Qty = "5",
                    UOM = "EA"
                },
                new PickLines
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-002",
                    Item_No = "ITM-TEST-2",
                    Open_Qty = "6",
                    UOM = "EA"
                },
                new PickLines
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-003",
                    Item_No = "ITM-TEST-3",
                    Open_Qty = "7",
                    UOM = "EA"
                },
                new PickLines
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-004",
                    Item_No = "ITM-TEST-4",
                    Open_Qty = "8",
                    UOM = "EA"
                }
            };

            // Fallback: muestra todo si no llega parámetro
            PickingDetails.ItemsSource = _allDocuments;
        }


        private async void PickingDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLine = e.CurrentSelection.FirstOrDefault() as PickLines;
            

            if (selectedLine == null)
                return;

            await Shell.Current.GoToAsync(
                "Picking_Line_Details",
                new Dictionary<string, object>
                {
            { "Line", selectedLine }
                });

            ((CollectionView)sender).SelectedItem = null;
        }



        /// <summary>
        /// Recibe parámetros desde Shell (SourceDocument)
        /// </summary>
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.TryGetValue("SourceDocument", out var value))
                return;

            var sourceDocument =
                Uri.UnescapeDataString(value?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(sourceDocument))
                return;

            System.Diagnostics.Debug.WriteLine(
                $"PICKING_TAKE | SourceDocument recibido: {sourceDocument}");

            var filtered = _allDocuments
                .Where(x =>
                    x.SourceDocument.Equals(
                        sourceDocument,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            System.Diagnostics.Debug.WriteLine(
                $"PICKING_TAKE | Líneas encontradas: {filtered.Count}");

            PickingDetails.ItemsSource = filtered;
        }
    }

    /// <summary>
    /// Modelo de líneas de picking
    /// </summary>
    public class PickLines
    {
        public string SourceDocument { get; set; }
        public string Bin { get; set; }
        public string Item_No { get; set; }
        public string Open_Qty { get; set; }
        public string UOM { get; set; }
    }
}
