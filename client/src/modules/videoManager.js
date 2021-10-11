import { getToken } from "./authManager";

const baseUrl = '/api/video';

export const getAllVideos = () => {
  return getToken().then((token) => {
    return fetch(baseUrl, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error("An unknown error occurred while trying to get videos.");
      }
    });
  });
};

export const getAllVideosWithComments = () => {
  return getToken().then((token) => {
    return fetch(`${baseUrl}/getwithcomments`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error("An unknown error occurred while trying to get videos.");
      }
    });
  });  
};

// export const searchVideos = (videoSearchTerm, sortDesc) => {
//     return fetch(`${baseUrl}/search?q=${videoSearchTerm}&sortDesc=${sortDesc}`)
//       .then((res) => res.json())
// };

export const searchVideos = (videoSearchTerm) => {
    console.log('searching?')
    return getToken().then((token) => {
      return fetch(`${baseUrl}/search?q=${videoSearchTerm}&sortDesc=false`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`
        }
      }).then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("An unknown error occurred while trying to search videos.");
        }
      });
    });    
};

export const addVideo = (video) => {
  return getToken().then((token) => {
  return fetch(baseUrl, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(video)
    }).then(res => {
      if (res.ok) {
        return res.json();
      } else if (res.status === 401) {
        throw new Error("Unauthorized");
      } else {
        throw new Error("An unknown error occurred while trying to save a new video.");
      }
    });
  });
};

export const getVideo = (id) => {
  return getToken().then((token) => {
    return fetch(`${baseUrl}/${id}`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error("An unknown error occurred while trying to get video.");
      }
    });
  });    
};

export const getVideosByUser = (id) => {
  console.log("getting user videos?")
  return getToken().then((token) => {
    return fetch(`${baseUrl}/users/${id}`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error("An unknown error occurred while trying to get user videos.");
      }
    });
  });  
};

