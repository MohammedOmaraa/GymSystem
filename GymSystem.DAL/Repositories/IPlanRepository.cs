using GymSystem.DAL.Entities;


namespace GymSystem.DAL.Repositories
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllPlansAsync();
        Task<Plan?> GetPlanByIdAsync(int id);
        void Add(Plan plan);
        void Update(Plan plan);
        void Delete(int id);
        Task<int> CompleteAsync();
    }
}
