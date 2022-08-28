using LinqKit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data.Repositories;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
    public interface IStatisticalService
    {
        Task<StatisticalDayDto> ByDay(string startDate, string endDate);

        Task<StatisticalDayDto> ByMonth(int month, int year);

        Task<StatisticalDayDto> ByYear(int year);
    }

    public class StatisticalService: IStatisticalService
    {
        IScheduleRepository _repository;
        public StatisticalService(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<StatisticalDayDto> ByDay(string startDate, string endDate)
        {
            Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
            

            var from = DateTime.Parse(startDate).AddDays(-1);
            baseFilter = baseFilter.And(x => DateTime.Compare(x.ScheduleDate, from) > 0);//>=0

            var to = DateTime.Parse(endDate).AddDays(1);
            baseFilter = baseFilter.And(x => DateTime.Compare(x.ScheduleDate, to) < 0);//<=0

            var total = await _repository.Count(baseFilter);
            var allSchedule = await _repository.GetMulti(baseFilter);
            var scheduleHasLetter = allSchedule.Count(x => x.ISendSMS || x.IsSendEmail);

            var row = new List<StatisticalChartRow>();
            // reset startDate
            from = from.AddDays(1);
            while (DateTime.Compare(from, to) != 0)
            {
                var series = new List<StatisticalChartSeries>();
                var totalHasLetterByDay = allSchedule.Count(x => x.ScheduleDate.Day == from.Day
                    && x.ScheduleDate.Month == from.Month
                    && x.ScheduleDate.Year == from.Year && (x.ISendSMS || x.IsSendEmail));

                series.Add(new StatisticalChartSeries() { 
                    Name = "Có thư mời",
                    Value = totalHasLetterByDay
                });

                var totalNoLetterByDay = allSchedule.Count(x => x.ScheduleDate.Day == from.Day
                    && x.ScheduleDate.Month == from.Month
                    && x.ScheduleDate.Year == from.Year && (!x.ISendSMS && !x.IsSendEmail));
                series.Add(new StatisticalChartSeries()
                {
                    Name = "Không có thư mời",
                    Value = totalNoLetterByDay
                });

                row.Add(new StatisticalChartRow() { 
                    Name = $"{from.ToString("dd/MM/yyyy")}",
                    Series = series
                });

                from = from.AddDays(1);
            }

            return new StatisticalDayDto()
            {
                TotalSchedule = total,
                TotalScheduleHasLetter = scheduleHasLetter,
                TotalScheduleNoLetter = total - scheduleHasLetter,
                ChartData = row
            };
        }

        public async Task<StatisticalDayDto> ByMonth(int month, int year)
        {
            Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;

            baseFilter = baseFilter.And(x => x.ScheduleDate.Month == month && x.ScheduleDate.Year == year);

            var total = await _repository.Count(baseFilter);
            var allSchedule = await _repository.GetMulti(baseFilter);
            var scheduleHasLetter = allSchedule.Count(x => x.ISendSMS || x.IsSendEmail);

            var now = new DateTime(year, month, 1);
            var nextMonth = now.AddMonths(1);

            var row = new List<StatisticalChartRow>();
            while (DateTime.Compare(now, nextMonth) != 0)
            {
                var series = new List<StatisticalChartSeries>();
                var totalHasLetterByDay = allSchedule.Count(x => x.ScheduleDate.Day == now.Day
                    && x.ScheduleDate.Month == now.Month
                    && x.ScheduleDate.Year == now.Year && (x.ISendSMS || x.IsSendEmail));

                series.Add(new StatisticalChartSeries()
                {
                    Name = "Có thư mời",
                    Value = totalHasLetterByDay
                });

                var totalNoLetterByDay = allSchedule.Count(x => x.ScheduleDate.Day == now.Day
                    && x.ScheduleDate.Month == now.Month
                    && x.ScheduleDate.Year == now.Year && (!x.ISendSMS && !x.IsSendEmail));
                series.Add(new StatisticalChartSeries()
                {
                    Name = "Không có thư mời",
                    Value = totalNoLetterByDay
                });

                row.Add(new StatisticalChartRow()
                {
                    Name = $"{now.ToString("dd/MM/yyyy")}",
                    Series = series
                });

                now = now.AddDays(1);
            }

            return new StatisticalDayDto()
            {
                TotalSchedule = total,
                TotalScheduleHasLetter = scheduleHasLetter,
                TotalScheduleNoLetter = total - scheduleHasLetter,
                ChartData = row
            };
        }

        public async Task<StatisticalDayDto> ByYear(int year)
        {
            Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;

            baseFilter = baseFilter.And(x => x.ScheduleDate.Year == year);

            var total = await _repository.Count(baseFilter);
            var allSchedule = await _repository.GetMulti(baseFilter);
            var scheduleHasLetter = allSchedule.Count(x => x.ISendSMS || x.IsSendEmail);

            var now = new DateTime(year, 1, 1);
            var nextYear = now.AddYears(1);

            var row = new List<StatisticalChartRow>();
            while (DateTime.Compare(now, nextYear) != 0)
            {
                var series = new List<StatisticalChartSeries>();
                var totalHasLetterByDay = allSchedule.Count(x =>
                    x.ScheduleDate.Month == now.Month
                    && x.ScheduleDate.Year == now.Year && (x.ISendSMS || x.IsSendEmail));

                series.Add(new StatisticalChartSeries()
                {
                    Name = "Có thư mời",
                    Value = totalHasLetterByDay
                });

                var totalNoLetterByDay = allSchedule.Count(x => x.ScheduleDate.Month == now.Month
                    && x.ScheduleDate.Year == now.Year && (!x.ISendSMS && !x.IsSendEmail));
                series.Add(new StatisticalChartSeries()
                {
                    Name = "Không có thư mời",
                    Value = totalNoLetterByDay
                });

                row.Add(new StatisticalChartRow()
                {
                    Name = $"Tháng {now.Month}",
                    Series = series
                });

                now = now.AddMonths(1);
            }

            return new StatisticalDayDto()
            {
                TotalSchedule = total,
                TotalScheduleHasLetter = scheduleHasLetter,
                TotalScheduleNoLetter = total - scheduleHasLetter,
                ChartData = row
            };
        }
    }
}
