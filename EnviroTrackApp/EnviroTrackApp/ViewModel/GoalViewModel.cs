using EnviroTrackApp.Model;
using EnviroTrackApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviroTrackApp.ViewModel
{
    public class GoalViewModel
    {
        private DatabaseContext getContext()
        {
            return new DatabaseContext();
        }

        public int InsertGoal(GoalModel goal)
        {
            var _dbContext = getContext();
            _dbContext.Goals.Add(goal);
            int c = _dbContext.SaveChanges();
            return c;
        }

        public async Task<List<GoalModel>> GetAllGoals()
        {
            var _dbContext = getContext();
            var res = await _dbContext.Goals.ToListAsync();
            return res;
        }

        public async Task<int> UpdateGoal(GoalModel goal)
        {
            var _dbContext = getContext();
            _dbContext.Goals.Update(goal);
            int c = await _dbContext.SaveChangesAsync();
            return c;
        }

        public async Task DeleteGoalAsync(GoalModel goal)
        {
            var _dbContext = getContext();
            _dbContext.Goals.Remove(goal);
            await _dbContext.SaveChangesAsync();
        }
    }
}