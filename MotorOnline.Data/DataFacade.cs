using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Data
{
    public class DataFacade
    {
        static DataFacade _data;
        public static DataFacade Data
        {
            get {
                if (_data == null)
                {
                    _data = new DataFacade();
                }
                return _data;
            }
        }

        private TransactionData _transactionData;
        public TransactionData TransactionData {
            get {
                if (_transactionData == null)
                {
                    _transactionData = new TransactionData();
                }
                return _transactionData;
            }
        }

        private DefaultPerilsData _defaultPerilsData;
        public DefaultPerilsData DefaultPerilsData
        {
            get
            {
                if (_defaultPerilsData == null)
                {
                    _defaultPerilsData = new DefaultPerilsData();
                }
                return _defaultPerilsData;
            }
        }

        private EndorsementData _endorsementData;
        public EndorsementData EndorsementData
        {
            get
            {
                if (_endorsementData == null)
                {
                    _endorsementData = new EndorsementData();
                }
                return _endorsementData;
            }
        }

        private TransactionPerilsData _transactionPerilsData;
        public TransactionPerilsData TransactionPerilsData
        {
            get
            {
                if (_transactionPerilsData == null)
                {
                    _transactionPerilsData = new TransactionPerilsData();
                }
                return _transactionPerilsData;
            }
        }

        private TransactionComputationData _transactionComputationData;
        public TransactionComputationData TransactionComputationData
        {
            get
            {
                if (_transactionComputationData == null)
                {
                    _transactionComputationData = new TransactionComputationData();
                }
                return _transactionComputationData;
            }
        }

        private CarDetailsData _carDetailsData;
        public CarDetailsData CarDetailsData
        {
            get
            {
                if (_carDetailsData == null)
                {
                    _carDetailsData = new CarDetailsData();
                }
                return _carDetailsData;
            }
        }

        private CustomerInfoData _customerInfoData;
        public CustomerInfoData CustomerInfoData
        {
            get
            {
                if (_customerInfoData == null)
                {
                    _customerInfoData = new CustomerInfoData();
                }
                return _customerInfoData;
            }
        }

        private MiscData _miscData;
        public MiscData MiscData
        {
            get
            {
                if (_miscData == null)
                {
                    _miscData = new MiscData();
                }
                return _miscData;
            }
        }
        
    }
}
