using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{


    class Program
    {


        static void Main(string[] args)
        {
            var adming =  UserController.GetUserByType(UserType.Admin);
            Console.WriteLine(adming.UserType);
        }
    }

    public abstract class User
    {
        public abstract UserType UserType { get; }
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }


        protected User(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }

        protected User()
        {
            
        }
    }

    public enum UserType
    {
        Admin
    }

    class Admin : User
    {
        public Admin(string firstName, string lastName) : base(firstName, lastName)
        {

        }

        public Admin()
        {
            
        }

        public override UserType UserType => UserType.Admin;
    }

    public static class UserController
    {
        public static  Dictionary<UserType, User> allUsers { get; private set; } = new Dictionary<UserType, User>();
        private static bool isInitialized = false;

        static UserController()
        {
            var userTypes = Assembly.GetAssembly(typeof(User)).GetTypes().Where(t => typeof(User).IsAssignableFrom(t) && !t.IsAbstract);
            
            foreach (var userType in userTypes)
            {
                if (Activator.CreateInstance(userType) is User user) allUsers.Add(user.UserType, user);
            }
            isInitialized = true;
        }

        public static User GetUserByType(UserType userType)
        {
            return allUsers[userType];
        }

    }
}
