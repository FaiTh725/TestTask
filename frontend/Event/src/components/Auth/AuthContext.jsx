import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({children}) => {
  const [user, setUser] = useState(null);
  const [currentUser, setCurrentUser] = useState(undefined);

  const login = (userData) => {
    setUser(userData);
  }

  const logout = () => {
    setUser(null);
    localStorage.removeItem("user");
  }

  useEffect(() => {
    try
    {
      const userData = JSON.parse(localStorage.getItem("user"));
      // console.log(userData);
      if(userData)
      {
        setUser(userData);
        setCurrentUser(userData);
      }
      else
      {
        setUser(null);
        setCurrentUser(null);
      }
    }
    catch
    {
      console.error("Error Read Storage");
      setUser(null);
      setCurrentUser(null);
    }
  }, [])

  if(currentUser === undefined)
  {
    return (
      <div style={{"width": "100vw", "height": "100vh", "display": "flex", "justifyContent": "center", "alignItems": "center"}}>
        loading
      </div>
    )
  }
  else 
  {
    return (
      <AuthContext.Provider value={{user, login, logout}}>
        {children}
      </AuthContext.Provider>
    )
  }
}

export const useAuth = () => useContext(AuthContext);