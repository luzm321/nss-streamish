using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }


        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, [Name], Email, ImageUrl, DateCreated
                          FROM UserProfile";

                    using var reader = cmd.ExecuteReader();

                    var userProfiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),

                        });
                    }

                    reader.Close();

                    return userProfiles;
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, Up.FirebaseUserId, up.Name AS UserProfileName, up.Email
                          FROM UserProfile up
                         WHERE FirebaseUserId = @FirebaseuserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Name = DbUtils.GetString(reader, "UserProfileName"),
                            Email = DbUtils.GetString(reader, "Email"),
                            //UserTypeId = DbUtils.GetInt(reader, "UserTypeId"),
                            //UserType = new UserType()
                            //{
                            //    Id = DbUtils.GetInt(reader, "UserTypeId"),
                            //    Name = DbUtils.GetString(reader, "UserTypeName"),
                            //}
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, [Name], Email, ImageUrl, DateCreated
                          FROM UserProfile
                          WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    using var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        };

                    }
                    //reader.Close();
                    return userProfile;
                }
            }
        }

        // Get UserProfile by Id with Videos:
        public UserProfile GetByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT up.Id, up.[Name], up.Email, up.ImageUrl, up.DateCreated,
                                 v.Id [VideoId], v.Title, v.Description, v.Url, v.DateCreated [VideoDateCreated],
                                 c.Id [CommentId], c.Message, c.UserProfileId [CommentUserProfileId], cup.[Name] [Commenter Name]
                          FROM UserProfile up
                          LEFT JOIN Video v
                          ON up.Id = v.UserProfileId
                          LEFT JOIN Comment c
                          ON v.Id = c.VideoId
                          LEFT JOIN UserProfile cup
                          ON c.UserProfileId = cup.Id
                          WHERE up.Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    using var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;
                    //while loop utilized because there may be more than 1 comment for each video
                    while (reader.Read())
                    {
                        //creating the UserProfile object if there isn't already one and if on the first row
                        if (userProfile == null)
                        {
                            userProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                Videos = new List<Video>()
                            };
                        }                      
                        // Checking if there is a Video in row:
                        if (DbUtils.IsNotDbNull(reader, "VideoId"))
                        {
                            //Linq search to check if the current row reader is on is already in the videos list to avoid duplicate videos:
                            var video = userProfile.Videos.FirstOrDefault(video => video.Id == DbUtils.GetInt(reader, "VideoId"));
                            //If video is not on the list, create new video object and add to list:
                            if (video == null)
                            {
                                video = new Video
                                {
                                    Id = DbUtils.GetInt(reader, "VideoId"),
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    Url = DbUtils.GetString(reader, "Url"),
                                    DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                    Comments = new List<Comment>()
                                };

                                userProfile.Videos.Add(video);
                            }
                            //Checking if there is a comment in row, create new comment object and add to list:
                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                video.Comments.Add(new Comment
                                {
                                    Id = DbUtils.GetInt(reader, "CommentId"),
                                    Message = DbUtils.GetString(reader, "Message"),
                                    //VideoId = video.Id,
                                    //UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                    UserProfile = new UserProfile()
                                    {
                                        //Id = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                        Name = DbUtils.GetString(reader, "Commenter Name"),
                                        //Email = DbUtils.GetString(reader, "Email"),
                                        //ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                                    }
                                });
                            }
                        }                       
                    }
                    //reader.Close();
                    return userProfile;
                }
            }
        }
    
        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (FirebaseUserId, Name, Email, ImageUrl, DateCreated)
                        OUTPUT INSERTED.ID
                        VALUES (@firebaseUserId, @name, @email, @imageUrl, @dateCreated)";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", userProfile.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET Name = @name,
                            Email = @email,
                            ImageUrl = @imageUrl,   
                            DateCreated = @dateCreated,
                         WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM UserProfile 
                                        WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
