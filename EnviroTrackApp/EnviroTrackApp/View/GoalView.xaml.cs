using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using static Xamarin.Essentials.Permissions;
using EnviroTrackApp.Model;
using EnviroTrackApp.ViewModel;
using EnviroTrackApp;

namespace EnviroTrackApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoalView : ContentPage
    {
        GoalViewModel _viewModel;
        string _selectedAction;
        string _category;
        string _description;
        string _actionCode;
        private GoalModel selectedGoal;

        // Constructor for creating a new goal
        public GoalView(string selectedAction)
        {
            InitializeComponent();
            _viewModel = new GoalViewModel();
            BindingContext = _viewModel;

            _selectedAction = selectedAction;
            _category = GetCategoryForAction(selectedAction);
            _description = GetDescriptionForAction(selectedAction);
            _actionCode = GetActionCodeForAction(selectedAction);

            selectedActionLabel.Text = "Action: " + _selectedAction;
            categoryLabel.Text = "Category: " + _category;
            descriptionLabel.Text = "Description: " + _description;
            actionCodeLabel.Text = "Action Code: " + _actionCode;

            // Set the predetermined impact level
            string predeterminedImpactLevel = GetImpactLevelForAction(_selectedAction);
            impactLevelLabel.Text = "Impact Level: " + predeterminedImpactLevel;
        }

        // Constructor for updating an existing goal
        public GoalView(GoalModel selectedGoal)
        {
            InitializeComponent();
            _viewModel = new GoalViewModel();
            BindingContext = _viewModel;

            this.selectedGoal = selectedGoal;
            _selectedAction = selectedGoal.SelectedAction;
            _category = selectedGoal.Category;
            _description = selectedGoal.Description;
            _actionCode = selectedGoal.ActionCode;

            selectedActionLabel.Text = "Action: " + _selectedAction;
            categoryLabel.Text = "Category: " + _category;
            descriptionLabel.Text = "Description: " + _description;
            actionCodeLabel.Text = "Action Code: " + _actionCode;
            impactLevelLabel.Text = "Impact Level: " + selectedGoal.ImpactLevel;

            // Set the selected frequency and frequency action
            frequencyPicker.SelectedItem = selectedGoal.Frequency;
            PopulateFrequencyActionPicker(selectedGoal.Frequency);
            frequencyActionPicker.SelectedItem = GetFrequencyActionForGoal(selectedGoal);

        }
        private void OnFrequencyChanged(object sender, EventArgs e)
        {
            string selectedFrequency = frequencyPicker.SelectedItem?.ToString();
            PopulateFrequencyActionPicker(selectedFrequency);
        }

        private void PopulateFrequencyActionPicker(string selectedFrequency)
        {
            frequencyActionPicker.ItemsSource = null; // Clear the ItemsSource before modifying Items

            switch (selectedFrequency)
            {
                case "Daily":
                    frequencyActionPicker.ItemsSource = new List<string> { "Once a day", "Twice a day", "Thrice a day", "Four times a day", "Five times a day", "Six times a day", "Seven times a day" };
                    break;
                case "Weekly":
                    frequencyActionPicker.ItemsSource = new List<string> { "Once a week", "Twice a week", "Thrice a week", "Four times a week", "Five times a week", "Six times a week", "Seven times a week" };
                    break;
                case "Monthly":
                    frequencyActionPicker.ItemsSource = new List<string> { "Five times a month", "10 (Ten) times a month", "15 (Fifteen) times a month", "20 (Twenty) times a month" };
                    break;
                case "Yearly":
                    frequencyActionPicker.ItemsSource = new List<string> { "30 (Thirty) times a year", "60 (Sixty) times a year", "90 (Ninety) times a year", "120 (One Hundred Twenty) times a year", "150 (One Hundred Fifty) times a year", "180 (One Hundred Eighty) times a year", "210 (Two Hundred Ten) times a year" };
                    break;
                default:
                    break;
            }
        }

        private string GetFrequencyActionForGoal(GoalModel goal)
        {
            // Retrieve the current frequency action based on goal details.
            // This is a placeholder and should be replaced with actual logic to map goal details to a frequency action.
            return goal.FrequencyAction;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var frequency = frequencyPicker.SelectedItem?.ToString();
            var frequencyAction = frequencyActionPicker.SelectedItem?.ToString();

            string predeterminedImpactLevel = GetImpactLevelForAction(_selectedAction);

            if (selectedGoal == null) // New goal
            {
                var goal = new GoalModel
                {
                    Category = _category,
                    SelectedAction = _selectedAction,
                    Description = _description,
                    Frequency = frequency,
                    FrequencyAction = frequencyAction,
                    PredeterminedImpactLevel = predeterminedImpactLevel,
                    ImpactLevel = CalculateImpactLevel(frequencyAction, predeterminedImpactLevel),
                    ActionCode = _actionCode
                };

                _viewModel.InsertGoal(goal);
            }
            else // Update existing goal
            {
                selectedGoal.Frequency = frequency;
                selectedGoal.FrequencyAction = frequencyAction;
                selectedGoal.ImpactLevel = CalculateImpactLevel(frequencyAction, predeterminedImpactLevel);
                _viewModel.UpdateGoal(selectedGoal);
            }

            Application.Current.MainPage = new MainPage();
        }

        private string GetCategoryForAction(string action)
        {
            return action switch
            {
                var a when a.StartsWith("Turn off lights when not in use") => "Energy",
                var a when a.StartsWith("Unplug electrical appliances") => "Energy",
                var a when a.StartsWith("Use energy-efficient appliances") => "Energy",
                var a when a.StartsWith("Install solar panels to generate renewable energy") => "Energy",
                var a when a.StartsWith("Use electric fans to reduce air conditioning use") => "Energy",
                var a when a.StartsWith("Reuse coffee grounds as fertilizer") => "Waste",
                var a when a.StartsWith("Regularly recycle paper, plastic, glass, and metal") => "Waste",
                var a when a.StartsWith("Use reusable bags for shopping") => "Waste",
                var a when a.StartsWith("Repurpose plastic bottles into plant pots") => "Waste",
                var a when a.StartsWith("Start composting organic waste") => "Waste",
                var a when a.StartsWith("Take shorter showers to reduce water usage") => "Water",
                var a when a.StartsWith("Repair dripping faucets and leaking toilets promptly") => "Water",
                var a when a.StartsWith("Collect rainwater for garden use") => "Water",
                var a when a.StartsWith("Save water in the laundry room") => "Water",
                var a when a.StartsWith("Use a water flask") => "Water",
                _ => "Unknown"
            };
        }

        private string GetDescriptionForAction(string action)
        {
            return action switch
            {
                "Turn off lights when not in use" => "This action typically results in a modest reduction in energy usage, especially if consistently practiced throughout the day.",
                "Unplug electrical appliances" => "Unplugging frequently used electronics like TVs and computers can lead to a significant reduction in energy usage, especially if done consistently.",
                "Use energy-efficient appliances" => "Switching to energy-efficient appliances can lead to substantial energy savings, particularly over the long term.",
                "Install solar panels to generate renewable energy" => "Solar panels have the potential to significantly reduce energy usage, especially if properly installed and maintained.",

                "Reuse coffee grounds as fertilizer" => "While reusing coffee grounds is beneficial, the impact on waste reduction may be relatively modest compared to other actions.",
                "Regularly recycle paper, plastic, glass, and metal" => "Recycling a significant portion of household waste can lead to substantial waste reduction, especially if consistently practiced.",
                "Use reusable bags for shopping" => "Switching to reusable bags can lead to noticeable waste reduction, especially if used frequently.",
                "Repurpose plastic bottles into plant pots" => "While repurposing plastic bottles is environmentally friendly, the impact on waste reduction may be relatively minor compared to other actions.",

                "Take shorter showers to reduce water usage" => "Shortening shower duration can result in notable water savings, especially if consistently practiced.",
                "Repair dripping faucets and leaking toilets promptly" => "Fixing leaks promptly can lead to substantial water savings, especially if addressing major leaks.",
                "Collect rainwater for garden use" => "While collecting rainwater is beneficial, its impact on water usage reduction may be relatively modest compared to other actions.",
                "Save water in the laundry room" => "Upgrading to a high-efficiency washing machine can lead to notable water savings, especially if consistently used for full loads.",

                _ => "Unknown Description"
            };
        }

        private string GetActionCodeForAction(string action)
        {
            return action switch
            {
                "Turn off lights when not in use" => "E001",
                "Unplug electrical appliances" => "E002",
                "Use energy-efficient appliances" => "E003",
                "Install solar panels to generate renewable energy" => "E004",

                "Reuse coffee grounds as fertilizer" => "W001",
                "Regularly recycle paper, plastic, glass, and metal" => "W002",
                "Use reusable bags for shopping" => "W003",
                "Repurpose plastic bottles into plant pots" => "W004",

                "Take shorter showers to reduce water usage" => "WTR001",
                "Repair dripping faucets and leaking toilets promptly" => "WTR002",
                "Collect rainwater for garden use" => "WTR003",
                "Save water in the laundry room" => "WTR004",
                _ => "Unknown"
            };
        }

        private string GetImpactLevelForAction(string action)
        {
            return action switch
            {
                "Turn off lights when not in use" => "Low",
                "Unplug electrical appliances" => "Low",
                "Use energy-efficient appliances" => "Medium",
                "Install solar panels to generate renewable energy" => "High",

                "Reuse coffee grounds as fertilizer" => "Low",
                "Repurpose plastic bottles into plant pots" => "Low",
                "Use reusable bags for shopping" => "Medium",
                "Regularly recycle paper, plastic, glass, and metal" => "High",

                "Take shorter showers to reduce water usage" => "Low",
                "Repair dripping faucets and leaking toilets promptly" => "Medium",
                "Collect rainwater for garden use" => "Low",
                "Save water in the laundry room" => "High",
                _ => "Unknown"
            };
        }

        private string CalculateImpactLevel(string frequencyAction, string currentImpactLevel)
        {
            // Define impact level thresholds based on frequency action
            Dictionary<string, Dictionary<string, string>> impactLevelThresholds = new Dictionary<string, Dictionary<string, string>>
            {
                ["Daily"] = new Dictionary<string, string>
                {
                    ["Once a day"] = "Low",
                    ["Twice a day"] = "Low",
                    ["Thrice a day"] = "Medium",
                    ["Four times a day"] = "Medium",
                    ["Five times a day"] = "Medium",
                    ["Six times a day"] = "High",
                    ["Seven times a day"] = "High"
                },
                ["Weekly"] = new Dictionary<string, string>
                {
                    ["Once a week"] = "Low",
                    ["Twice a week"] = "Low",
                    ["Thrice a week"] = "Medium",
                    ["Four times a week"] = "Medium",
                    ["Five times a week"] = "Medium",
                    ["Six times a week"] = "High",
                    ["Seven times a week"] = "High"
                },
                ["Monthly"] = new Dictionary<string, string>
                {
                    ["Five times a month"] = "Low",
                    ["10 (Ten) times a month"] = "Low",
                    ["15 (Fifteen) times a month"] = "Medium",
                    ["20 (Twenty) times a month"] = "Medium",
                    ["25 (Twenty Five) times a month"] = "High",
                    ["30 (Thirty) times a month"] = "High"

                },
                ["Yearly"] = new Dictionary<string, string>
                {
                    ["30 (Thirty) times a year"] = "Low",
                    ["60 (Sixty) times a year"] = "Low",
                    ["90 (Ninety) times a year"] = "Medium",
                    ["120 (One Hundred Twenty) times a year"] = "Medium",
                    ["150 (One Hundred Fifty) times a year"] = "High",
                    ["180 (One Hundred Eighty) times a year"] = "High",
                    ["210 (Two Hundred Ten) times a year"] = "High"
                }
            };

            // Determine the new impact level based on frequency action and current impact level
            switch (currentImpactLevel)
            {
                case "High":
                    return "High"; // No modification if already High

                case "Medium":
                    if (frequencyAction == "Six times a day" || frequencyAction == "Seven times a day" ||
                        frequencyAction == "Six times a week" || frequencyAction == "Seven times a week" ||
                        frequencyAction == "150 (One Hundred Fifty) times a year" ||
                        frequencyAction == "180 (One Hundred Eighty) times a year" ||
                        frequencyAction == "210 (Two Hundred Ten) times a year")
                    {
                        return "High";
                    }
                    return "Medium"; // Remains Medium if frequency action does not justify High

                case "Low":
                    if (frequencyAction == "Thrice a day" || frequencyAction == "Four times a day" ||
                        frequencyAction == "Five times a day" || frequencyAction == "Thrice a week" ||
                        frequencyAction == "Four times a week" || frequencyAction == "Five times a week" ||
                        frequencyAction == "15 (Fifteen) times a month" ||
                        frequencyAction == "20 (Twenty) times a month" ||
                        frequencyAction == "90 (Ninety) times a year" ||
                        frequencyAction == "120 (One Hundred Twenty) times a year")
                    {
                        return "Medium";
                    }
                    else if (frequencyAction == "Six times a day" || frequencyAction == "Seven times a day" ||
                             frequencyAction == "Six times a week" || frequencyAction == "Seven times a week" ||
                             frequencyAction == "150 (One Hundred Fifty) times a year" ||
                             frequencyAction == "180 (One Hundred Eighty) times a year" ||
                             frequencyAction == "210 (Two Hundred Ten) times a year")
                    {
                        return "High";
                    }
                    return "Low"; // Remains Low if frequency action does not justify Medium or High

                default:
                    return currentImpactLevel; // If current impact level is not recognized, do not change it
            }
        }
    }
}
