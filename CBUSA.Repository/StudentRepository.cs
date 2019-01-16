using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {

        public StudentRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
        public Student GetTopRankStudent(int Year)
        {
            throw new NotImplementedException();
        }

        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }


    }
}
