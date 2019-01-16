using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBUSA.Repository
{
   public interface  IStudentRepository :IRepository<Student>
    {
        Student GetTopRankStudent(int Year);
      
   
    }
}
