using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Paper_Rock.Models;

using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;

namespace Paper_Rock.Controllers
{
    public class RoomsController : Controller
    {

        private DBContext _db = new DBContext();
        
        [ChildActionOnly]
        public ActionResult MyCreatedRooms()
        {

            return PartialView(GetMyCreatedRooms());
        }
       [ChildActionOnly]
        public ActionResult MyJoinedRooms()
        {

            return PartialView(GetMyJoinedRooms());
        }

        // GET: Rooms
        [Authorize]
        public ActionResult Index()
        {

            return View(GetRooms());
        }


        // GET: Rooms/Edit/5
        public ActionResult Join(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = _db.Rooms.Include(e => e.Player1).Where(e => e.Id == id).FirstOrDefault();
            if (room == null)
            {
                return HttpNotFound();
            }
            if (room.Player1.UserName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            if(room.InGame == true)
            {
                return RedirectToAction("Details", id);
            }
            if (room.Player2Id != User.Identity.GetUserId())
            {

                if (room.Player2Id == null)
                {
                    if (!SendSecondPlayer(room))
                        return RedirectToAction("Index");
                }
                else return RedirectToAction("Index");
            }

            return View(room);
        }

     
        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join([Bind(Include = "Id,Player2Res,InGame,Value")] Room room)
        {
            var updateroom = _db.Rooms.Where(e => e.Id == room.Id).Include(e => e.Player1).Include(e => e.Player2).FirstOrDefault();
            updateroom.Player2Res = room.Player2Res;
            updateroom.InGame = room.InGame;
            if (TryUpdateModel(updateroom))
            {
                try
                {
                    SetResult(updateroom);
                    _db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Rooms/Details/5
        public ActionResult  Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = _db.Rooms.Where(e=>e.Id == id).Include(e=>e.Player1).Include(e=>e.Player2).FirstOrDefault();
            if (room == null)
            {
                return HttpNotFound();
            }
            if (!room.InGame)
            {
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Rooms/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }


        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Player1Id,Player1Res,Value")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.Player1Id = User.Identity.GetUserId();
                room.Player1 = await _db.Users.Where(e => e.UserName == User.Identity.Name).FirstOrDefaultAsync();
                if (room.Value > room.Player1.Points)
                {
                    ViewBag.Message = "Don't have enought points";
                    return View(room);
                }
                AddRoom(room);
                return RedirectToAction("Index");
            }

            return View(room);
        }


        [Authorize]
        public ActionResult Statistics()
        {
            string userId = User.Identity.GetUserId();
            var rooms = _db.Rooms.Where(e => e.Player1Id.Contains(userId)).Where(e=>e.InGame == true).ToList();
            var rooms2 = _db.Rooms.Where(e => e.Player2Id.Contains(userId)).Where(e => e.InGame == true).ToList();

            foreach (var item in rooms2)
                rooms.Add(item);

            List<RoomsStatistic> stats = new List<RoomsStatistic>();
           
            
            foreach (var items in rooms)
            {
                var roomstat = new RoomsStatistic();
                roomstat.room = items;
                roomstat.Id = userId;
                roomstat.win = roomstat.setBool();

                stats.Add(roomstat);
            

            };
            
            

            return View(stats);
        }


        //// GET: Rooms/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Room room = await db.Rooms.FindAsync(id);
        //    if (room == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(room);
        //}

        //// POST: Rooms/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Room room = await db.Rooms.FindAsync(id);
        //    db.Rooms.Remove(room);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}



        private void AddRoom(Room room)
        {
            _db.Rooms.Add(room);
            var user = _db.Users.Where(e => e.UserName == room.Player1.UserName).FirstOrDefault();
            user.Points -= room.Value;

            if (TryUpdateModel(user))
            {
                try
                {
                    _db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
        }

        private List<Room> GetRooms()
        {
            string userid = User.Identity.GetUserId().ToString();
            var rooms = _db.Rooms.Where(e => e.Player2Id == null).Where(e => e.Player1Id != userid).Include(e => e.Player1).OrderBy(e=>e.Value).ToList();

            return rooms;

        }

        
        private void SetResult(Room room)
        {

            Result res1 = room.Player1Res;
            Result res2 = room.Player2Res;

            if (res1 == res2)
            {
                room.Player1.Points += room.Value;
                room.Player2.Points += room.Value;
            }
            else
            {

                switch (res1)
                {
                    case Result.Rock:
                        {
                            if (res2 == Result.Scissors)
                                room.Player1.AddUserPoints(room.Value * 2);
                            if (res2 == Result.Paper)
                                room.Player2.AddUserPoints(room.Value * 2);
                        }
                        break;
                    case Result.Scissors:
                        {
                            if (res2 == Result.Paper)
                                room.Player1.AddUserPoints(room.Value * 2);
                            if (res2 == Result.Rock)
                                room.Player2.AddUserPoints(room.Value * 2);
                        }
                        break;

                    case Result.Paper:
                        {
                            if (res2 == Result.Scissors)
                                room.Player2.AddUserPoints(room.Value * 2);
                            if (res2 == Result.Rock)
                                room.Player1.AddUserPoints(room.Value * 2);
                        }
                        break;
                }
            }

        }


        private List<Room> GetMyCreatedRooms()
        {
            string userid = User.Identity.GetUserId().ToString();
            var rooms = _db.Rooms.Where(e => e.Player1Id == userid).Where(e => e.InGame == false).Include(e => e.Player1).ToList();

            return rooms;
        }


        private List<Room> GetMyJoinedRooms()
        {
            string userid = User.Identity.GetUserId().ToString();
            var rooms = _db.Rooms.Where(e => e.Player2Id == userid).Include(e => e.Player1).Where(e => e.InGame == false).ToList();

            return rooms;
        }


        private bool SendSecondPlayer(Room room)
        {
            

            room.Player2 = _db.Users.Where(e => e.UserName == User.Identity.Name).FirstOrDefault();
            room.Player2Id = User.Identity.GetUserId();
            if (room.Player2.Points < room.Value)
                return false;
            room.Player2.Points -= room.Value;
            if (TryUpdateModel(room))
            {
                try
                {
                    _db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return true;


        }




    }
}
   




