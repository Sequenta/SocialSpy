﻿namespace SocialSpy.Domain
{
    public interface ISocialService
    {
        UserInfo GetUserInfo(string user);
        FriendsInfo GetFriendsInfo(string user);
    }
}