using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using Dapper;
using System.Data.SqlClient;

namespace WcfDbEchoLib
{
    class ChequeRepository : IChequeRepository
    {
        string ConnectionString { get; }

        public ChequeRepository()
        {
           
            ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            // ConnectionString = ConfigurationManager.ConnectionStrings["DupperDbConnection"].ConnectionString;
        }

        /// <summary>
        /// Возвращает N-последних чеков из БД
        /// </summary>
        /// <param name="lastChequeCount"></param>
        /// <returns></returns>
        public List<Cheque> GetLastCheques(int lastChequeCount)
        {
            List<Cheque> chequesCollection = new List<Cheque>();

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Cheque cheque;
                var cheques1 = db.Query("dbo.get_cheques_pack", new { pack_size = lastChequeCount }, commandType: CommandType.StoredProcedure).ToList();
                foreach (var e in cheques1)
                {
                    var splitedArticles = e.articles.Split(';');

                    cheque = new Cheque()
                    {
                        Id = e.cheque_id,
                        Number = e.cheque_number,
                        Summ = e.summ,
                        Discount = e.discount,
                        Articles = splitedArticles
                    };

                    chequesCollection.Add(cheque);
                }
                return chequesCollection;
            }

           
        }

        /// <summary>
        /// Принимает чек и сохраняет в БД
        /// </summary>
        /// <param name="cheque"></param>
        public void SaveCheque(Cheque cheque)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Execute("dbo.save_cheques", new
                {
                    cheque_id = cheque.Id,
                    cheque_number = cheque.Number,
                    summ = cheque.Summ,
                    discount = cheque.Discount,
                    articles = string.Join(";", cheque.Articles)
                }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
