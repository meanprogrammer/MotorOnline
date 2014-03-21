using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;

namespace MotorOnline.Data
{
    public static class SqlGenerator
    {
        internal static string CreateSQLforPerils(List<TransactionPeril> perils, int transactionId)
        {

            StringBuilder sql = new StringBuilder();
            foreach (TransactionPeril p in perils)
            {
                sql.AppendFormat(
                    "INSERT INTO [dbo].[mTransactionPerils] ([TransactionID], [PerilID], [LimitSI], " +
                    "[Rate], [Premium], [PolicyRate], [PolicyPremium]) VALUES ({0}, {1}, " +
                    "{2}, {3}, {4}, {5}, {6});",
                    transactionId,
                    p.PerilID,
                    p.NewLimitSI,
                    p.NewRate,
                    p.NewPremium,
                    p.NewPolicyRate,
                    p.NewPolicyPremium);
            }
            return sql.ToString();
        }

        internal static string CreateSQLforComputations(ComputationDetails net,
                                                ComputationDetails gross, int transactionId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("INSERT INTO [dbo].[mTransactionComputations] ([TransactionID], " +
                            "[netBasicPremium], [netDocStamps], [netVat], [netDstonCoc], [netLtoCon], " +
                            "[netTotalAmmountDue], [grossBasicPremium], [grossDocStamps], [grossVat], " +
                            "[grossDstonCoc], [grossLtoCon], [grossTotalAmmountDue]) VALUES " +
                            "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12});",
                            transactionId,
                            net.BasicPremium, net.DocumentaryStamps, net.ValueAddedTax, net.DSTonCOC,
                            net.LTOInterconnectivity, net.GrandTotal,
                            gross.BasicPremium, gross.DocumentaryStamps, gross.ValueAddedTax, gross.DSTonCOC,
                            gross.LTOInterconnectivity, gross.GrandTotal);
            return sql.ToString();


        }
    }
}
