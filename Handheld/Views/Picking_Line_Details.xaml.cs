using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Handheld.Views
{
    public partial class Picking_Line_Details : ContentPage, IQueryAttributable
    {
        private List<PickLinesDetails> _allDocuments;

        private PickLinesDetails _line;

        public Picking_Line_Details(PickLinesDetails line)
        {
            InitializeComponent();
            _line = line;
            BindingContext = _line;
        }

        public Picking_Line_Details()
        {
            InitializeComponent();

            // Datos de ejemplo (luego vendrán de BC / API)
            _allDocuments = new List<PickLinesDetails>
            {
                new PickLinesDetails
                {
                    SourceDocument = "PI000004",
                    Bin = "W-01-001",
                    Item_No = "ITM-TEST-1",
                    Open_Qty = "5",
                    UOM = "EA"
                },
                new PickLinesDetails
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-002",
                    Item_No = "ITM-TEST-2",
                    Open_Qty = "6",
                    UOM = "EA"
                },
                new PickLinesDetails
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-003",
                    Item_No = "ITM-TEST-3",
                    Open_Qty = "7",
                    UOM = "EA"
                },
                new PickLinesDetails
                {
                    SourceDocument = "W202510-6 0027",
                    Bin = "W-01-004",
                    Item_No = "ITM-TEST-4",
                    Open_Qty = "8",
                    UOM = "EA"
                }
            };
        }       

            public void ApplyQueryAttributes(IDictionary<string, object> query)
            {
                if (!query.TryGetValue("Line", out var value))
                    return;

                if (value is not PickLines line)
                    return;

                BindingContext = line;
            } 



    /// <summary>
    /// Modelo de líneas de picking
    /// </summary>
    public class PickLinesDetails
        {
            public string SourceDocument { get; set; }
            public string Bin { get; set; }
            public string Item_No { get; set; }
            public string Open_Qty { get; set; }
            public string UOM { get; set; }
        }
    }
}
