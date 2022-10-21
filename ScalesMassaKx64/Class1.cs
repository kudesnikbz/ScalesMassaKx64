using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
//using System.Runtime.InteropServices;
using MassaKDriver100;

public class ScalesMassaKx64
{
    [SqlProcedure]
    public static void GetWeight_MassaKx64(
    SqlString IPAddress,
    //SqlString Port,
    out SqlBoolean StableFlag,
    out SqlDecimal Weight,
    out SqlString Error)
    {
        StableFlag = (SqlBoolean)false;
        Weight = SqlDecimal.Null;
        Error = SqlString.Null;
        Scales Scales = new Scales();
        try
        {
            Scales.Connection = IPAddress.ToString();
            int num = Scales.OpenConnection();
            if (num != 0)
            {
                Error = (SqlString)("Ошибка подключения к весам: " + IPAddress.ToString() + " " + num.ToString());
                StableFlag = (SqlBoolean)false;
                return;
            }
        }
        catch (Exception ex)
        {
            Error = (SqlString)("Ошибка работы с весами (неизвестная):" + ("Exception: {0}", ex.Message).ToString());
            StableFlag = (SqlBoolean)false;
            return;
        }
        try
        {
            int num1 = Scales.ReadWeight();
            if (num1 == 0)
            {
                int division = Scales.Division;
                Decimal num2 = 1.0M;
                switch (division)
                {
                    case 0:
                        num2 = 0.000001M;
                        break;
                    case 1:
                        num2 = 0.001M;
                        break;
                    case 2:
                        num2 = 0.01M;
                        break;
                    case 3:
                        num2 = 0.1M;
                        break;
                    case 4:
                        num2 = 1M;
                        break;
                }
                if (Scales.Stable == 1)
                {
                    Decimal weight = (Decimal)Scales.Weight;
                    Scales.CloseConnection();
                    Weight = (SqlDecimal)(num2 * weight);
                }
            }
            else
            {
                Scales.CloseConnection();
                Error = (SqlString)("Ошибка полученния веса:" + num1.ToString());
                StableFlag = (SqlBoolean)false;
                return;
            }
        }
        catch (Exception ex)
        {
            Error = (SqlString)("Ошибка работы с весами (неизвестная):" + ("Exception: {0}", ex.Message).ToString());
            StableFlag = (SqlBoolean)false;
            return;
        }
        StableFlag = (SqlBoolean)true;
        Scales.CloseConnection();
    }
}