// File: Application/Dashboard/Queries/GetDashboardData/GetDashboardDataResponse.cs

using CorrespondenceTracker.Application.Correspondences.Queries.GetCorrespondences;
using CorrespondenceTracker.Application.Reminders.Queries.GetReminders;

namespace CorrespondenceTracker.Application.Correspondences.Queries.GetDashboardData
{
    public class GetDashboardDataResponse
    {
        // 1- The number of total correspondences
        public int TotalCorrespondenceCount { get; set; }

        // 2- The number of open correspondences
        public int OpenCorrespondenceCount { get; set; }

        // 3- The first 10 open correspondences
        public List<GetCorrespondenceItemResponse> Top10OpenCorrespondences { get; set; } = new();

        // 4- All Today's reminders that are not completed and whose time has passed
        public List<GetReminderResponse> OverdueRemindersToday { get; set; } = new();
    }
}