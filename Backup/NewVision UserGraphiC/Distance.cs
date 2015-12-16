using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Distance
{
    static double[] use_rg_X;
    static double[] use_rg_Y;
    static double[] use_X;
    static double[] use_Y;

    public double lineSpace(double x1, double y1, double x2, double y2)
    {
        double lineLength = 0;

        lineLength = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));


        return lineLength;
    }


    public double LineDistance(double x1, double y1, double x2, double y2, double x0, double y0)
    {
        //x0 y0到(x1 y1 x2 y2)所連成直線的距離
        double space = 0;

        double a, b, c;

        a = lineSpace(x1, y1, x2, y2);

        b = lineSpace(x1, y1, x0, y0);

        c = lineSpace(x2, y2, x0, y0);

        if (c <= 0.000001 || b <= 0.000001)
        {   //在線上

            space = 0;

            return space;

        }

        if (a <= 0.000001)
        {

            space = b;

            return space;

        }

        if (c * c >= a * a + b * b)
        {

            space = b;

            return space;

        }

        if (b * b >= a * a + c * c)
        {

            space = c;

            return space;

        }

        double p = (a + b + c) / 2;

        double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

        space = 2 * s / a;

        return space;

    }


    public static int get_line_ab(double[] A, double[] B, double[] type_a,double[] type_b)
    {
        //把最短距離算出來平均
        int use = 0;
        double count = 0;
        double dd = 0;
        double Min = 99999999;
        bool flag = false;
        int abc = 0;

        use_rg_X = type_a;
        use_rg_Y = type_b;
        use_X = A;
        use_Y = B;


        for (int j = 0; j < use_X.Length; j++)
        {

            for (int i = 0; i < use_rg_X.Length; i++)
            {

                if (i != use_rg_X.Length - 1)
                {
                    if ((check(use_rg_X[i], use_rg_Y[i], use_rg_X[i + 1], use_rg_Y[i + 1], use_X[j], use_Y[j]) == false))
                    {
                        flag = true;
                        dd = new Distance().LineDistance(use_rg_X[i], use_rg_Y[i], use_rg_X[i + 1], use_rg_Y[i + 1], use_X[j], use_Y[j]);
                        if (dd < Min)
                        {
                            Min = dd;
                        }


                    }
                }
                else
                {
                    if ((check(use_rg_X[i], use_rg_Y[i], use_rg_X[0], use_rg_Y[0], use_X[j], use_Y[j]) == false))
                    {
                        flag = true;

                        dd = new Distance().LineDistance(use_rg_X[i], use_rg_Y[i], use_rg_X[0], use_rg_Y[0], use_X[j], use_Y[j]);
                        if (dd < Min)
                        {
                            Min = dd;
                        }
                    }
                }

            }
            if (flag == true)
            {
                count += Min;
                abc++;
                Min = 99999999;
                flag = false;
            }
        }
        use = (int)Math.Round(count);
        try
        {
            use = use / abc;
        }
        catch (Exception ex) { return 0; }
        return use;
    }


    public static bool check(double a, double b, double c, double d, double x, double y)
    {
        //找出點是在範圍外面還裡面
        bool TF = true;

        double result = ((d - b) * x - (c - a) * y + (c * b - a * d)) * (c*b - a*d);

        if (result < 0)
        {

            TF = false;

        }

        return TF;

    }


    public static int get_line_ab_2(double[] A, double[] B, double[] type_a, double[] type_b)
    {
        //把最短距離算出來平均
        int use = 0;
        double count = 0;
        double dd = 0;
        double Min = 99999999;
        bool flag = false;
        int abc = 0;

        use_rg_X = type_a;
        use_rg_Y = type_b;
        use_X = A;
        use_Y = B;


        for (int j = 0; j < use_X.Length; j++)
        {

            for (int i = 0; i < use_rg_X.Length; i++)
            {

                if (i != use_rg_X.Length - 1)
                {
                    if ((check(use_rg_X[i], use_rg_Y[i], use_rg_X[i + 1], use_rg_Y[i + 1], use_X[j], use_Y[j]) == false))
                    {
                        flag = true;
                        dd = Math.Sqrt(use_X[j] * use_X[j] + use_Y[j] * use_Y[j]);
                        if (dd < Min)
                        {
                            Min = dd;
                        }


                    }
                }
                else
                {
                    if ((check(use_rg_X[i], use_rg_Y[i], use_rg_X[0], use_rg_Y[0], use_X[j], use_Y[j]) == false))
                    {
                        flag = true;
                        dd = Math.Sqrt(use_X[j] * use_X[j] + use_Y[j] * use_Y[j]);
                        if (dd < Min)
                        {
                            Min = dd;
                        }
                    }
                }

            }
            if (flag == true)
            {
                count += Min;
                abc++;
                Min = 99999999;
                flag = false;
            }
        }
        use = (int)Math.Round(count);
        try
        {
            use = use / abc;
        }
        catch (Exception ex) { return 0; }
        return use;
    }
}