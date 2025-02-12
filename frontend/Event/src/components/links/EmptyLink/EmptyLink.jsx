import { useNavigate } from "react-router-dom";
import styles from "./EmptyLink.module.css";

const EmptyLink = ({children, link}) => {
  const navigate = useNavigate();

  return (
    <span className={styles.EmptyLink__Main} 
      onClick={() => navigate(link)}>
      {children}
    </span>
  )
}

export default EmptyLink;