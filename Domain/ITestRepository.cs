using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface ITestRepository
    {
        List<TestModel> GetAll();
        void AddTest(TestModel test);
        void EditTest(TestModel test);
        void DeleteTest(TestModel test);

    }
}
