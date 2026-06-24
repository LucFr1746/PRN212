using System;
using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.Services;
using Xunit;

namespace Assignment.Tests
{
    public class BookingValidationTests
    {
        private readonly BookingService _bookingService;

        public BookingValidationTests()
        {
            _bookingService = new BookingService();
        }

        #region Date Range Validation

        [Fact]
        public void ValidateBookingDetail_StartDateEqualsEndDate_ThrowsException()
        {
            var detail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 1)
            };

            var exception = Assert.Throws<Exception>(() =>
                _bookingService.ValidateBookingDetail(detail, new List<BookingDetail>()));

            Assert.Contains("Start Date must be before End Date", exception.Message);
        }

        [Fact]
        public void ValidateBookingDetail_StartDateAfterEndDate_ThrowsException()
        {
            var detail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 5),
                EndDate = new DateOnly(2099, 7, 1)
            };

            var exception = Assert.Throws<Exception>(() =>
                _bookingService.ValidateBookingDetail(detail, new List<BookingDetail>()));

            Assert.Contains("Start Date must be before End Date", exception.Message);
        }

        [Fact]
        public void ValidateBookingDetail_StartDateInThePast_ThrowsException()
        {
            var detail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2020, 1, 1),
                EndDate = new DateOnly(2020, 1, 5)
            };

            var exception = Assert.Throws<Exception>(() =>
                _bookingService.ValidateBookingDetail(detail, new List<BookingDetail>()));

            Assert.Contains("Start Date cannot be in the past", exception.Message);
        }

        #endregion

        #region Local Overlap Detection

        [Fact]
        public void ValidateBookingDetail_SameRoomOverlappingDatesInLocalList_ThrowsException()
        {
            var existingDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 5)
            };

            var newDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 3),
                EndDate = new DateOnly(2099, 7, 7)
            };

            var currentDetails = new List<BookingDetail> { existingDetail };

            var exception = Assert.Throws<Exception>(() =>
                _bookingService.ValidateBookingDetail(newDetail, currentDetails));

            Assert.Contains("overlapping date range", exception.Message);
        }

        [Fact]
        public void ValidateBookingDetail_SameRoomCheckoutEqualsNextCheckin_NoLocalOverlap()
        {
            // Boundary: Check-out date equals next Check-in date should NOT conflict.
            // Room 999 is booked 7/1 -> 7/3. New booking 7/3 -> 7/5 should be allowed locally.
            var existingDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 3)
            };

            var newDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 3),
                EndDate = new DateOnly(2099, 7, 5)
            };

            var currentDetails = new List<BookingDetail> { existingDetail };

            // This should pass local validation (no local overlap).
            // It may throw on the DB overlap check if there's a DB conflict,
            // but that's a database-level concern, not local validation.
            // We verify by checking no "overlapping date range" exception is thrown.
            try
            {
                _bookingService.ValidateBookingDetail(newDetail, currentDetails);
                // If no exception, the local overlap check passed.
            }
            catch (Exception ex)
            {
                // Only fail if the exception is about local overlap
                Assert.DoesNotContain("overlapping date range", ex.Message);
            }
        }

        [Fact]
        public void ValidateBookingDetail_DifferentRoomSameDatesInLocalList_NoOverlap()
        {
            // Different rooms should never conflict locally, even with identical date ranges.
            var existingDetail = new BookingDetail
            {
                RoomId = 100,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 5)
            };

            var newDetail = new BookingDetail
            {
                RoomId = 200,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 5)
            };

            var currentDetails = new List<BookingDetail> { existingDetail };

            try
            {
                _bookingService.ValidateBookingDetail(newDetail, currentDetails);
            }
            catch (Exception ex)
            {
                // Should not fail on local overlap for different rooms
                Assert.DoesNotContain("overlapping date range", ex.Message);
            }
        }

        [Fact]
        public void ValidateBookingDetail_SameRoomCompletelyContainedDates_ThrowsException()
        {
            // Existing: 7/1 -> 7/10. New: 7/3 -> 7/5 (fully inside). Should overlap.
            var existingDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 1),
                EndDate = new DateOnly(2099, 7, 10)
            };

            var newDetail = new BookingDetail
            {
                RoomId = 999,
                StartDate = new DateOnly(2099, 7, 3),
                EndDate = new DateOnly(2099, 7, 5)
            };

            var currentDetails = new List<BookingDetail> { existingDetail };

            var exception = Assert.Throws<Exception>(() =>
                _bookingService.ValidateBookingDetail(newDetail, currentDetails));

            Assert.Contains("overlapping date range", exception.Message);
        }

        #endregion

        #region Shared Validation Helper

        [Theory]
        [InlineData("0969442579", true)]
        [InlineData("123456789", true)]
        [InlineData("123456789012", true)]
        [InlineData("12345678", false)]      // 8 digits - too short
        [InlineData("1234567890123", false)] // 13 digits - too long
        [InlineData("abc", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidPhone_ReturnsExpectedResult(string? phone, bool expected)
        {
            bool result = Shared.ValidationHelper.IsValidPhone(phone!);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user@domain.org", true)]
        [InlineData("invalid", false)]
        [InlineData("@missing.com", false)]
        [InlineData("no spaces@test.com", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidEmail_ReturnsExpectedResult(string? email, bool expected)
        {
            bool result = Shared.ValidationHelper.IsValidEmail(email!);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
