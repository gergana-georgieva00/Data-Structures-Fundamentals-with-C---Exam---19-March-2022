namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        private Dictionary<string, Coupon> couponesByCodes = new Dictionary<string, Coupon>();
        private Dictionary<string, Website> websitesByDomains = new Dictionary<string, Website>();

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.websitesByDomains.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            websitesByDomains.Add(website.Domain, website);
            website.Coupons.Add(coupon);
        }

        public bool Exist(Website website)
            => this.websitesByDomains.ContainsKey(website.Domain);

        public bool Exist(Coupon coupon)
            => this.couponesByCodes.ContainsKey(coupon.Code);

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!websitesByDomains.ContainsKey(website.Domain));
            {
                throw new ArgumentException();
            }


        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Website> GetSites()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
        {
            throw new NotImplementedException();
        }

        public void RegisterSite(Website website)
        {
            throw new NotImplementedException();
        }

        public Coupon RemoveCoupon(string code)
        {
            throw new NotImplementedException();
        }

        public Website RemoveWebsite(string domain)
        {
            throw new NotImplementedException();
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}
