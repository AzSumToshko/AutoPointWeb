using AutoPoint.Tools;
using AutoPoint.ViewModel.CommentVM;
using AutoPoint.Entity;
using AutoPoint.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AutoPoint.Controllers
{
    public class CommentController : Controller
    {
        private readonly ModelMapper modelMapper;
        private readonly CommentRepository commentRepository;

        public CommentController()
        {
            this.modelMapper = new ModelMapper();
            this.commentRepository = new CommentRepository();
        }

        /// <summary>
        ///         This action returns the user to the products details page after
        ///         sending comment to the products details
        /// </summary>
        public IActionResult AddComment(CreateCommentVM model)
        {
            //checks if the input is valid
            if (!ModelState.IsValid)
                return RedirectToAction("Details", "Product", new { id = model.productID });

            //createing and adding the comment to the database
            Comment comment = modelMapper.mapCommentVMToComment(model);
            commentRepository.addComment(comment);

            //returning to the view
            return RedirectToAction("Details", "Product", new { id = comment.productID });
        }
    }
}
