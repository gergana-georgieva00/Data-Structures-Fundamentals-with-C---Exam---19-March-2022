namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            website.Coupons.Add(coupon);
            couponesByCodes.Add(coupon.Code, coupon);
        }

        public bool Exist(Website website)
            => this.websitesByDomains.ContainsKey(website.Domain);

        public bool Exist(Coupon coupon)
            => this.couponesByCodes.ContainsKey(coupon.Code);

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!websitesByDomains.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            return website.Coupons;
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
            => couponesByCodes.Values.OrderByDescending(c => c.Validity).ThenByDescending(c => c.DiscountPercentage);

        public IEnumerable<Website> GetSites()
            => websitesByDomains.Values;

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
            => websitesByDomains.Values.OrderBy(w => w.UsersCount).ThenByDescending(w => w.Coupons.Count);

        public void RegisterSite(Website website)
        {
            if (websitesByDomains.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            websitesByDomains.Add(website.Domain, website);
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!couponesByCodes.ContainsKey(code))
            {
                throw new ArgumentException();
            }

            var coupon = couponesByCodes[code];
            couponesByCodes.Remove(code);
            foreach (var website in websitesByDomains.Values)
            {
                website.Coupons.Remove(coupon);
            }

            return coupon;
        }

        public Website RemoveWebsite(string domain)
        {
            if (!websitesByDomains.ContainsKey(domain))
            {
                throw new ArgumentException();
            }

            var website = websitesByDomains[domain];
            websitesByDomains.Remove(domain);

            return website;
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if (!websitesByDomains.ContainsKey(website.Domain) || !couponesByCodes.ContainsKey(coupon.Code))
            {
                throw new ArgumentException();
            }
            if (!website.Coupons.Contains(coupon))
            {
                throw new ArgumentException();
            }

            website.Coupons.Remove(coupon);
            couponesByCodes.Remove(coupon.Code);
        }
    }
}
