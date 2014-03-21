using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Business
{
    public class BusinessFacade
    {
        static BusinessFacade _business;
        public static BusinessFacade Business
        {
            get 
            {
                if (_business == null)
                {
                    _business = new BusinessFacade();
                }
                return _business;
            }
        }

        private CustomerInfoBusiness _customerInfoBusiness;
        public CustomerInfoBusiness CustomerInfoBusiness 
        {
            get 
            {
                if (_customerInfoBusiness == null)
                {
                    _customerInfoBusiness = new CustomerInfoBusiness();
                }
                return _customerInfoBusiness;
            }
        }

        private DefaultPerilsBusiness _defaultPerilsBusiness;
        public DefaultPerilsBusiness DefaultPerilsBusiness
        {
            get
            {
                if (_defaultPerilsBusiness == null)
                {
                    _defaultPerilsBusiness = new DefaultPerilsBusiness();
                }
                return _defaultPerilsBusiness;
            }
        }

        private EndorsementBusiness _endorsementBusiness;
        public EndorsementBusiness EndorsementBusiness
        {
            get
            {
                if (_endorsementBusiness == null)
                {
                    _endorsementBusiness = new EndorsementBusiness();
                }
                return _endorsementBusiness;
            }
        }

        private TransactionBusiness _transactionBusiness;
        public TransactionBusiness TransactionBusiness
        {
            get
            {
                if (_transactionBusiness == null)
                {
                    _transactionBusiness = new TransactionBusiness();
                }
                return _transactionBusiness;
            }
        }

        private UserBusiness _userBusiness;
        public UserBusiness UserBusiness
        {
            get
            {
                if (_userBusiness == null)
                {
                    _userBusiness = new UserBusiness();
                }
                return _userBusiness;
            }
        }

        private MiscBusiness _miscBusiness;
        public MiscBusiness MiscBusiness
        {
            get
            {
                if (_miscBusiness == null)
                {
                    _miscBusiness = new MiscBusiness();
                }
                return _miscBusiness;
            }
        }
    }
}
