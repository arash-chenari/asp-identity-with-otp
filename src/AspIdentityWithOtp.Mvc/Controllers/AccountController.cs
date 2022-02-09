using System;
using System.Threading.Tasks;
using AspIdentityWithOtp.Mvc.Infrastructure;
using AspIdentityWithOtp.Mvc.Models;
using AspIdentityWithOtp.Mvc.Models.Repositories;
using AspIdentityWithOtp.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspIdentityWithOtp.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private const string _VERIFICATION_CODE = "123";
        
        private readonly ISmsService _smsService;
        private readonly IUserVerificationCodeRepository _userVerificationRepository;
        private readonly ITempUserRegistrationRepository _tempUserRegistrationRepository;
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(ISmsService smsService,
            UserManager<User> userManger,
            SignInManager<User> signInManager,
            IUserVerificationCodeRepository userVerificationRepository
            , ITempUserRegistrationRepository empUserRegistrationRepository)
        {
            _smsService = smsService;
            _userManger = userManger;
            _signInManager = signInManager;
            _userVerificationRepository = userVerificationRepository;
            _tempUserRegistrationRepository = empUserRegistrationRepository;
        }

        [HttpGet]
        public ViewResult Login(string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await _userManger.FindByNameAsync(model.PhoneNumber);
            var verificationCode = _VERIFICATION_CODE;
            var verificationCodeModel = new UserVerificationCode
            {
                Code = verificationCode,
                GeneratedCodeDateTime = System.DateTime.UtcNow,
                UserId = user.Id
            };

            await _userVerificationRepository.Add(verificationCodeModel);
            _smsService.Send(user.PhoneNumber, verificationCode);

            return RedirectToAction(nameof(VerifyUser),user.Id);
        }

        [HttpGet]
        public  IActionResult VerifyUser(string userId)
        {
            ViewBag.UserId = userId;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyUser(UserVerificationCodeViewModel model)
        {
            var user = await _userManger.FindByIdAsync(model.UserId);

            if (user == null)
            {
                throw new ArgumentNullException("user with this id is null");
            }

            var lastVerificationCodeModel = await _userVerificationRepository.GetLastVerificationCode(user.Id);

            if (DateTime.UtcNow.Subtract(lastVerificationCodeModel.GeneratedCodeDateTime).TotalMinutes > 2)
            {
                throw new ArgumentNullException("verification code is expierd");
            }

            if (model.Code != lastVerificationCodeModel.Code)
            {
                throw new ArgumentNullException("Wrong verification code");
            }

            await _signInManager.SignInAsync(user, true);
            return Redirect("http://localhost:5000/home/index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (await _userManger.FindByNameAsync(model.PhoneNumber) != null)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                var tempUser = new TempUserRegistration
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedCodeDateTime = DateTime.UtcNow,
                    PhoneNumber = model.PhoneNumber,
                    VerificationCode = _VERIFICATION_CODE
                };

                await _tempUserRegistrationRepository.Add(tempUser);
                //send verification code via sms

                ViewBag.PhoneNumber = model.PhoneNumber;
                return View("VerifyUserRegistration");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyUserRegistration(VerifyUserRegistrationViewModel model)
        {
            if (await _userManger.FindByNameAsync(model.PhoneNumber) != null)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                var lastTemUser = await _tempUserRegistrationRepository
                    .GetLastByPhoneNumber(model.PhoneNumber);

                if (lastTemUser == null)
                {
                    return RedirectToAction(nameof(Register));
                }

                if (DateTime.UtcNow.Subtract(lastTemUser.GeneratedCodeDateTime).TotalMinutes > 2)
                {
                    return View(nameof(Register));
                }

                var user = new User
                {
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.PhoneNumber,
                    CountryCallingCode = "0098",
                    Email = "a@b.com"
                };

                await _userManger.CreateAsync(user);
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index","Home");
            }
        }
    }
}
