using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class TestRepository : ITestRepository
    {
        private List<TestModel> TestList = new List<TestModel>();

        public TestRepository()
        {
            TestList = new List<TestModel>()
                {
                 new TestModel { Id = 1, Name="Test1", IsAvailable = true, DateFrom = DateTime.Now},
                 new TestModel { Id = 2, Name="Test2", IsAvailable = false, DateFrom = DateTime.UtcNow}
                 };
        }
        public void AddTest(TestModel test)
        {
            test.Id = TestList.Count > 0 ? TestList.Max(x => x.Id) + 1 : 1;
            TestList.Add(test);
        }

        public void DeleteTest(TestModel test)
        {
            TestList.Remove(test);
        }

        public void EditTest(TestModel test)
        {
            TestList.RemoveAll(x => x.Id == test.Id);
            TestList.Add(test);
        }

        public List<TestModel> GetAll()
        {
            return TestList;
        }

        public TestModel GetOne(int id)
        {
            return TestList.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
