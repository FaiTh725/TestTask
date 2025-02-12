import styles from "./Header.module.css";
import CenterScreen from "../layots/CenterScreen/CenterScreen";
import logo from "../../assets/Logo.png";
import EmptyLink from "../links/EmptyLink/EmptyLink";
import { useAuth } from "../Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import ClearButton from "../buttons/ClearButton/ClearButton";

const Header = () => {
  const auth = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    auth.logout();
    navigate("/account/login");
  }

  return(
    <div className={styles.Header__Main}>
      <CenterScreen>
        <EmptyLink link={"/"}>
          <img src={logo} alt="logo" width={80} height={80}/>
        </EmptyLink>
      </CenterScreen>
      {auth.user == null ?
        <div className={styles.Header__Navigation}>
          <EmptyLink link={"/account/login"}>
            <p>Sing in</p>
          </EmptyLink>
        </div> :

        <div className={styles.Header__Navigation}>
        <ClearButton action={handleLogout}>
          <p className={styles.Header__LogoutContent}>Log out</p>
        </ClearButton>
        <EmptyLink link={"/account/login"}>
          <p>My Events</p>
        </EmptyLink>
      </div>
      }
    </div>
  )
}

export default Header;