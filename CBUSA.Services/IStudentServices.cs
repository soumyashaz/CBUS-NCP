using System;
namespace CBUSA.Services
{
  public interface IStudentServices
    {
        System.Collections.Generic.IEnumerable<CBUSA.Domain.Student> GetStudent();
        void SaveStudent(CBUSA.Domain.Student ObjStudent);
        
    }
}
