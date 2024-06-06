using EnviroTrackApp.View;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnviroTrackApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionSelectionPage : ContentPage
    {
        public List<Action> ListDetails { get; set; }
        public ActionSelectionPage()
        {
            InitializeComponent();
            ListDetails = new List<Action>
            {
                new Action { ActionName = "Turn off lights when not in use", ActionIcon = "https://cdn-icons-png.flaticon.com/128/9986/9986785.png" },
                new Action { ActionName = "Unplug electrical appliances", ActionIcon = "https://cdn-icons-png.flaticon.com/128/7014/7014506.png" },
                new Action { ActionName = "Use energy-efficient appliances", ActionIcon = "https://cdn-icons-png.flaticon.com/128/11652/11652088.png" },
                new Action { ActionName = "Install solar panels to generate renewable energy", ActionIcon = "https://cdn-icons-png.flaticon.com/128/401/401124.png" },
                new Action { ActionName = "Reuse coffee grounds as fertilizer", ActionIcon = "https://cdn-icons-png.flaticon.com/128/2194/2194906.png" },
                new Action { ActionName = "Repurpose plastic bottles into plant pots", ActionIcon = "https://cdn-icons-png.flaticon.com/128/4173/4173479.png" },
                new Action { ActionName = "Use reusable bags for shopping", ActionIcon = "https://cdn-icons-png.flaticon.com/128/4343/4343265.png" },
                new Action { ActionName = "Regularly recycle paper, plastic, glass, and metal", ActionIcon = "https://cdn-icons-png.flaticon.com/128/5699/5699561.png" },
                new Action { ActionName = "Take shorter showers to reduce water usage", ActionIcon = "https://cdn-icons-png.flaticon.com/128/840/840208.png" },
                new Action { ActionName = "Collect rainwater for garden use", ActionIcon = "https://cdn-icons-png.flaticon.com/128/11531/11531245.png" },
                new Action { ActionName = "Repair dripping faucets and leaking toilets promptly", ActionIcon = "https://cdn-icons-png.flaticon.com/128/15186/15186020.png" },
                new Action { ActionName = "Save water in the laundry room", ActionIcon = "https://cdn-icons-png.flaticon.com/128/4352/4352835.png" }
            };

            BindingContext = this;
        }

        private async void OnActionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedAction = e.SelectedItem as Action;
                if (selectedAction != null)
                {
                    await Navigation.PushAsync(new GoalView(selectedAction.ActionName));
                }

                 // Deselect the item
                 ((ListView)sender).SelectedItem = null;
            }
        }

        public class Action
        {
            public string ActionName { get; set; }
            public string ActionIcon { get; set; }
        }
    }
}