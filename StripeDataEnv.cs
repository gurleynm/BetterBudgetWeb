namespace BetterBudgetWeb
{
    public class StripeDataEnv
    {
        public static bool StripeTestBool = false;
        public static string StripePortal => StripeTestBool ? "https://billing.stripe.com/p/login/test_6oE9AJfV35xS32U000" : "https://billing.stripe.com/p/login/7sIcQl0678fq0Xm7ss";
        public static string TableId => StripeTestBool ? "prctbl_1Qmj0L2L4K66u9tvWc1Wd8LN" : "prctbl_1Q3mJO2L4K66u9tvqO60pKZf";
        public static string PubKey => StripeTestBool ? "pk_test_51Q2DNf2L4K66u9tvGcuQV9KPwvSuJcwF3EVTNtBIQnHoX3VrX21cXVodQsBGiwFSvhVfZ2arUohyahSMBiafS4FU00YdP854MT" : "pk_live_51Q2DNf2L4K66u9tvbL1k0RCr1ANZKE2RIoHXS2Ob5v2cmsv9ZOLX09YgeYWv97vq1nMjHLTGOrGfNAlFcTxHQVnJ00VAthxA0G";
    }
}
