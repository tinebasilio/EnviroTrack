using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System;
using System.Threading.Tasks;
using EnviroTrackApp.ViewModel;
using System.Collections.ObjectModel;
using EnviroTrackApp.Model;
using EnviroTrackApp.View;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EnviroTrackApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardView : ContentPage
    {
        private GoalViewModel _viewModel;
        private int _currentPoints;
        private int _currentLevel;
        private const int PointsPerLevel = 100;

        public ObservableCollection<GoalModel> Goals { get; private set; }

        public DashboardView()
        {
            InitializeComponent();
            _viewModel = new GoalViewModel();
            Goals = new ObservableCollection<GoalModel>();
            goalsListView.ItemsSource = Goals;

            LoadProgress();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadGoals();
        }

        private async Task LoadGoals()
        {
            var goals = await _viewModel.GetAllGoals();
            Goals.Clear();
            foreach (var goal in goals)
            {
                Goals.Add(goal);
            }
        }

        private async void OnGoalSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedGoal = (GoalModel)e.SelectedItem;
                string res = await DisplayActionSheet("Operation", "Cancel", null, "Update Frequency", "Delete");

                switch (res)
                {
                    case "Update Frequency":
                        await Navigation.PushAsync(new GoalView(selectedGoal));
                        break;

                    case "Delete":
                        await _viewModel.DeleteGoalAsync(selectedGoal);
                        await LoadGoals();
                        break;
                }

                goalsListView.SelectedItem = null;
            }
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            // No need to navigate as we are already on the Dashboard
        }

        private async void OnBrowseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActionSelectionPage());
        }

        private async void OnMoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var checkbox = sender as CheckBox;
                var goal = checkbox.BindingContext as GoalModel;
                int points = 0;

                switch (goal.ImpactLevel.ToLower())
                {
                    case "low":
                        points = 15;
                        break;
                    case "medium":
                        points = 25;
                        break;
                    case "high":
                        points = 35;
                        break;
                }

                AddPoints(points);
                Goals.Remove(goal);
                await _viewModel.DeleteGoalAsync(goal); // Remove the goal from the database
            }
        }

        private void AddPoints(int points)
        {
            _currentPoints += points;

            while (_currentPoints >= PointsPerLevel)
            {
                _currentPoints -= PointsPerLevel;
                _currentLevel++;
            }

            SaveProgress();

            double progress = (double)_currentPoints / PointsPerLevel;
            levelProgressBar.Progress = progress;
            levelLabel.Text = $"Level {_currentLevel}";
            pointsProgressLabel.Text = $"{_currentPoints}/{PointsPerLevel}";
        }

        private void OnResetLevelClicked(object sender, EventArgs e)
        {
            _currentPoints = 0;
            _currentLevel = 1;
            SaveProgress();
            levelProgressBar.Progress = 0;
            levelLabel.Text = "Level 1";
            pointsProgressLabel.Text = "Points: 0/100"; // Update points display
        }

        private void LoadProgress()
        {
            if (Application.Current.Properties.ContainsKey("CurrentPoints"))
            {
                _currentPoints = (int)Application.Current.Properties["CurrentPoints"];
            }
            else
            {
                _currentPoints = 0;
            }

            if (Application.Current.Properties.ContainsKey("CurrentLevel"))
            {
                _currentLevel = (int)Application.Current.Properties["CurrentLevel"];
            }
            else
            {
                _currentLevel = 1;
            }

            double progress = (double)_currentPoints / PointsPerLevel;
            levelProgressBar.Progress = progress;
            levelLabel.Text = $"Level {_currentLevel}";
        }

        private void SaveProgress()
        {
            Application.Current.Properties["CurrentPoints"] = _currentPoints;
            Application.Current.Properties["CurrentLevel"] = _currentLevel;
            Application.Current.SavePropertiesAsync();
        }
    }
}
