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
using WcfDbEchoLib.Logger;

namespace WcfDbEchoLib
{
    class ChequeRepository : IChequeRepository
    {
        string ConnectionString { get; }

        public ChequeRepository()
        {    
            //Условное соединение с БД согласно заданию 
            ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        }

        /// <summary>
        /// Возвращает N чеков из БД посредством вызова хранимой процедуры dbo.get_cheques_pack
        /// </summary>
        /// <returns></returns>
        public List<Cheque> GetChequesPack(int packCount)
        {
            MyLogger.Log.Info($"Getting cheques Pack from DB");

            List<Cheque> chequesCollection = new List<Cheque>();
          
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Cheque cheque;
                var cheques1 = db.Query("dbo.get_cheques_pack", new { pack_size = packCount }, commandType: CommandType.StoredProcedure).ToList();
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
        /// Сохраняет чек в БД
        /// </summary>
        /// <param name="cheque"></param>
        public void SaveCheque(Cheque cheque)
        {
            MyLogger.Log.Info($"Save cheque to DB");

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
