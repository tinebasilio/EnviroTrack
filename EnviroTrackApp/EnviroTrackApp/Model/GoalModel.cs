using System;
using System.Collections.Generic;
using System.Text;

namespace EnviroTrackApp.Model
{
    public class GoalModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SelectedAction { get; set; }
        public string ImpactLevel { get; set; }
        public string Frequency { get; set; }
        public string ActionCode { get; set; }
        public string FrequencyAction { get; set; }
        public string PredeterminedImpactLevel { get; set; }

    }
}

