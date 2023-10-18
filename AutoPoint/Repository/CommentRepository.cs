using AutoPoint.DataBaseAccess;
using AutoPoint.Entity;

namespace AutoPoint.Repository
{
    public class CommentRepository
    {
        private readonly Context context;

        public CommentRepository()
        {
            this.context = new Context();
        }

        /// <summary>
        ///         removeAll removes all comments from the database.
        /// </summary>
        public void removeAll()
        {
            foreach (var item in context.Comments)
            {
                context.Comments.Remove(item);
            }
            context.SaveChanges();
        }

        /// <summary>
        ///         addComment gets a comment as a parameter and if the comment isnt null it gets 
        ///         inserted into the database
        /// </summary>
        public void addComment(Comment comment)
        {
            if (comment != null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method returns a list with all the comments that a product have
        /// </summary>
        public List<Comment> getComments(int id)
        {
            return context.Comments.Where(x => x.productID == id).ToList();
        }
    }
}
