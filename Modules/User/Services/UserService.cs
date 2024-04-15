using CourseWork.Common.database.Interfaces;
using CourseWork.Modules.user.repository;
using CourseWork.Modules.User.Dtos;
using CourseWork.Modules.User.Entity;

namespace CourseWork.Modules.User.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        //private readonly IDataBaseBaseInterface<UserEntity> _userRepo;
        public UserService(UserRepository userRepo) {
            _userRepo = userRepo;
        }

        public async Task<UserEntity> CreateUser(UserCreateDto data)
        {
            UserEntity userDataToSend = new UserEntity { Name =data.Name};
            UserEntity createdUser = await _userRepo.CreateAsync(userDataToSend, true);
            return createdUser;
        }
        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _userRepo.FindByIdAsync(id);
        }
    }
}
