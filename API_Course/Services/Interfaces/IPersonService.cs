﻿using MVC.Data.VO;
using System;
using System.Collections.Generic;

namespace MVC.Business
{
    public interface IPersonService : IDisposable
    {
        PersonVO Create(PersonVO person);
        PersonVO FindByID(long id);
        List<PersonVO> FindAll();
        PersonVO Update(PersonVO person);
        PersonVO Disable(long Id);
        void Delete(long id);
    }
}
