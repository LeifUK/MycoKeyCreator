using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace MycoKeyCreator.WebApplication.Controllers
{
    public class KeysController : Controller
    {
        public KeysController(Services.IKeyManagerFactory iKeyManagerFactory)
        {
            _iKeyManagerFactory = iKeyManagerFactory;
        }

        private Services.IKeyManagerFactory _iKeyManagerFactory;

        public IActionResult Index()
        {
            return View(_iKeyManagerFactory.GetKeyManager().GetKeyEnumerator().Where(n => n.Publish).ToList());
        }

        public IActionResult About()
        {
            return View("About");
        }

        public IActionResult MycoKeyCreator()
        {
            return View();
        }

        public IActionResult KeyMatch(string keyName)
        {
            Model.KeyMatchViewModel keyMatchViewModel = Model.KeyMatchViewDataBuilder.Build(_iKeyManagerFactory.GetKeyManager(), keyName, null);
            if (keyMatchViewModel != null)
            {
                return View(keyMatchViewModel);
            }

            return View();
        }

        /*
         * Invoked by the KeyMatch view
         */
        public IActionResult Update(Model.KeyMatchViewOutput keyMatchViewOutput)
        {
            if (keyMatchViewOutput == null)
            {
                return View();
            }

            Model.KeyMatchViewModel keyMatchViewModel = Model.KeyMatchViewDataBuilder.Build(_iKeyManagerFactory.GetKeyManager(), keyMatchViewOutput.KeyName, keyMatchViewOutput);
            if (keyMatchViewModel != null)
            {
                return View("KeyMatch", keyMatchViewModel);
            }

            return View();
        }
    }
}
