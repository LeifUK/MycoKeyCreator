using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace MycoKeys.WebApplication.Controllers
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
            return View(_iKeyManagerFactory.GetKeyManager().GetKeyEnumerator().ToList());
        }

        public IActionResult KeyMatch(string keyName)
        {
            Model.KeyMatchViewData keyMatchData = Model.KeyMatchViewDataBuilder.Build(_iKeyManagerFactory.GetKeyManager(), keyName, null);
            if (keyMatchData != null)
            {
                return View(keyMatchData);
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

            Dictionary<Int64, bool> selectionsMap = keyMatchViewOutput.Selections.ToDictionary(n => n.AttributeId, n => n.IsSelected);
            Model.KeyMatchViewData keyMatchData = Model.KeyMatchViewDataBuilder.Build(_iKeyManagerFactory.GetKeyManager(), keyMatchViewOutput.KeyName, selectionsMap);
            if (keyMatchData != null)
            {
                return View("KeyMatch", keyMatchData);
            }

            return View();
        }
    }
}
