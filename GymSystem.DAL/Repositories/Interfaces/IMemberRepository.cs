using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface IMemberRepository:IGrnericRepository<Member>
    {
        // New Features
        //Task<Member> GetMemberWithHealthRecordById(int id);
    }
}
