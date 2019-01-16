using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
   public class StudentServices : CBUSA.Services.IStudentServices
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public StudentServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<Student> GetStudent()
        {
          //  return _ObjUnitWork. IEnumerable<Student>
            //return null;

            var v = _ObjUnitWork.Students.GetAll();

            return _ObjUnitWork.Students.GetAll();
        }

       

        public void SaveStudent(Student ObjStudent)
        {
            //  return _ObjUnitWork. IEnumerable<Student>
            //return null;
           // return _ObjUnitWork.Students.GetAll();

            _ObjUnitWork.Students.Add(ObjStudent);
            _ObjUnitWork.Complete();
        }
    
    
    
    }
}
