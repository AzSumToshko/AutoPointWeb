using AutoPoint.DataBaseAccess;
using AutoPoint.Entity;
using Scrypt;
using System.Data.Entity;

namespace AutoPoint.Repository
{
    public class UserRepository
    {
        private readonly Context context;


        public UserRepository()
        {
            context = new Context();
        }

        /// <summary>
        ///         This method returns boolean representing if the user is existing or not
        /// </summary>
        public bool IsExisting(int id)
        {
            User user = context.Users.Find(id);
            return user != null;
        }

        /// <summary>
        ///         This method returns user with email and password matching the input.
        ///         If there are not users found retunrs null.
        /// </summary>
        public User FindByEmailAndPassword(string email , string password)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            foreach (var user in context.Users)
            {
                try//if the result of the search is error then the password isnt encoded and the catch finds it
                {
                    if (user.email.Equals(email) && encoder.Compare(password,user.password))
                    {
                        return user;
                    }
                }
                catch
                {
                    if (user.email.Equals(email) && user.password.Equals(password))
                    {
                        return user;
                    }
                }
                
            }
            return null;
        }

        /// <summary>
        ///         This method returns a user found by his email
        /// </summary>
        public User findByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.email == email);
        }

        /// <summary>
        ///         This method generates new password
        /// </summary>
        public string generateNewPassword(User user)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            Random random = new Random();
            string newPassword = "";

            newPassword += (char)random.Next(65, 90);//uppercase letter
            for (int i = 0; i < 3; i++)
            {
                newPassword += (char)random.Next(97,122);//lowercase letter
            }

            for (int i = 0; i < 4; i++)
            {
                newPassword += random.Next(0,9);//number
            }
            
            user.password = encoder.Encode(newPassword);

            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();

            return newPassword;
        }

        /// <summary>
        ///         This method returns the count of users
        /// </summary>
        public int Count()
        {
            return context.Users.Count();
        }

        /// <summary>
        ///         This method returns a user found by his unique id
        /// </summary>
        public User FindById(int id)
        {
            return context.Users.FirstOrDefault(i => i.ID == id);
        }

        /// <summary>
        ///         This method returns list with all the users
        /// </summary>
        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        /// <summary>
        ///         This method adds the user into the database
        /// </summary>
        public void AddToDataBase(User user)
        {
            if (user != null)
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method removes user from the database
        /// </summary>
        public User RemoveById(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return user;
        }

        /// <summary>
        ///         This method updates the user from the database
        /// </summary>
        public void Update(User user)
        {
            if (user != null)
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         removeAll removes all the users from the database
        /// </summary>
        public void removeAll()
        {
            foreach (var item in context.Users)
            {
                context.Users.Remove(item);
            }
            context.SaveChanges();
        }
    }
}
